using CodeNetUI_Example.Pages;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using MudBlazor;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodeNetUI_Example.Services;

public class ApiClientService(HttpClient httpClient, LocalStorageManager localStorageManager, IDialogService dialogService, ISnackbar snackbar)
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

    public async Task<T?> SendAsync<T>(HttpMethod method, string url, object? context, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage? httpResponse = null;
        if (method == HttpMethod.Get)
            httpResponse = await GetAsync(url, cancellationToken);
        else if (method == HttpMethod.Post)
            httpResponse = await PostAsync(url, context, cancellationToken);
        else if (method == HttpMethod.Put)
            httpResponse = await PutAsync(url, context, cancellationToken);
        else if (method == HttpMethod.Delete)
            httpResponse = await DeleteAsync(url, cancellationToken);
        else if (method == HttpMethod.Patch)
            httpResponse = await PatchAsync(url, JsonContent.Create(context, options: JsonOptions), cancellationToken);

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

    public async Task<HttpResponseMessage?> SendAsync(HttpMethod method, string url, object? context, CancellationToken cancellationToken = default)
    {
        try
        {
            using HttpClient httpClient = new();
            HttpRequestMessage request = new(method, url);
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            request.SetBrowserRequestMode(BrowserRequestMode.NoCors);
            if (context is not null)
            {
                request.Content = JsonContent.Create(context, options: JsonOptions);
            }

            var response = await httpClient.SendAsync(request, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }

    public async Task<HttpResponseMessage?> PostAsync(string url, object? context, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.PostAsJsonAsync(url, context, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }

    public async Task<HttpResponseMessage?> GetAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var response = await httpClient.GetAsync(url, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }

    public async Task<HttpResponseMessage?> DeleteAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var response = await httpClient.DeleteAsync(url, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }

    public async Task<HttpResponseMessage?> PutAsync(string url, object? context, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var response = await httpClient.PutAsJsonAsync(url, context, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }

    public async Task<HttpResponseMessage?> PatchAsync(string url, HttpContent? context, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await localStorageManager.GetAsync("authToken");
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var response = await httpClient.PatchAsync(url, context, cancellationToken);
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
        catch (Exception ex)
        {
            snackbar.Add(ex.Message, Severity.Error);
            return null;
        }
    }
}
