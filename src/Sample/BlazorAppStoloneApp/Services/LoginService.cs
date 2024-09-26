using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Models;
using MudBlazor;

namespace CodeNetUI_Example.Services;

public class LoginService(ApiClientService apiClientService, LocalStorageManager localStorageManager, ISnackbar snackbar)
{
    private const string _authToken = "authToken";

    public async Task<TokenModel?> GenerateToken(string username, string password, CancellationToken cancellationToken = default)
    {
        await localStorageManager.RemoveAsync(_authToken);
        var result = await apiClientService.SendAsync<TokenModel>(HttpMethod.Post, $"{AppSettings.LoginBaseUrl}/Token", new
        {
            Username = username,
            Password = password
        }, cancellationToken);
        if (result is not null)
        {
            snackbar.Add("Login successfull", Severity.Info);
            await localStorageManager.SetAsync(_authToken, result.Token);
            return result;
        }

        return null;
    }

    public async Task Logout()
    {
        await localStorageManager.RemoveAsync(_authToken);
        snackbar.Add("Logout!", Severity.Info);
    }
}
