using MudBlazor;
using System.Text;
using CodeNetUI_Example.Pages;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CodeNetUI_Example.Services;

public class ApiClientService(HttpClient http, LocalStorageManager localStorageManager, IDialogService dialogService, ISnackbar snackbar)
{
    private static JsonSerializerOptions JsonOptions
    {
        get
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
    }

    public async Task<T?> SendService<T>(HttpMethod method, string url, object? context, CancellationToken cancellationToken = default)
    {
        var httpResponse = await SendService(method, url, context, cancellationToken);

        try
        {
            if (httpResponse is not null && httpResponse.IsSuccessStatusCode)
            {
                var responseText = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                return JsonSerializer.Deserialize<T>(responseText, JsonOptions);
            }
            return default;
        }
        catch
        {
            snackbar.Add("Deserialize erorr!", Severity.Error);
            return default;
        }
    }

    public async Task<HttpResponseMessage?> SendService(HttpMethod method, string url, object? context, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpRequestMessage request = new(method, url);
            request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            if (context is not null)
            {
                var requestJson = JsonSerializer.Serialize(context, JsonOptions);
                request.Content = new StringContent(requestJson, Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json);
            }

            var response = await http.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            if (response is null)
            {
                snackbar.Add("Not response!", Severity.Warning);
                return null;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    snackbar.Add("Unauthorized!", Severity.Error);
                    await dialogService.ShowAsync<Login>("Login", new DialogOptions
                    {
                        FullWidth = true,
                        MaxWidth = MaxWidth.Small,
                        BackdropClick = false
                    });
                    return null;
                }
                else if (!response.IsSuccessStatusCode)
                {
                    var responseText = await response.Content.ReadAsStringAsync(cancellationToken);
                    snackbar.Add(responseText, Severity.Error);
                    return null;
                }

                return response;
            }
        }
        catch (HttpRequestException ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }
}
