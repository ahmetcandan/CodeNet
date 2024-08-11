using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Models;
using Microsoft.JSInterop;
using System.Text;

namespace CodeNetUI_Example.Services;

public class LoginService(HttpClient http, IJSRuntime jsRuntime)
{
    public async Task<TokenModel?> GenerateToken(string username, string password, CancellationToken cancellationToken = default)
    {
        var result = await http.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"{AppSettings.LoginBaseUrl}/Login")
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                Username = username,
                Password = password
            }), Encoding.UTF8, System.Net.Mime.MediaTypeNames.Application.Json)
        }, cancellationToken);
        var jsonResponse = await result.Content.ReadAsStringAsync(cancellationToken);
        var response = System.Text.Json.JsonSerializer.Deserialize<TokenModel>(jsonResponse);
        if (response is not null)
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.Token);
        return response;
    }

    public async Task<string> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
    }

    public async Task RemoveTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }
}
