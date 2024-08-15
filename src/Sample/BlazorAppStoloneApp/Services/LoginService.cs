using CodeNetUI_Example.Configurations;
using CodeNetUI_Example.Models;
using MudBlazor;

namespace CodeNetUI_Example.Services;

public class LoginService(ApiClientService apiClientService, LocalStorageManager localStorageManager, ISnackbar snackbar)
{
    public async Task<TokenModel?> GenerateToken(string username, string password, CancellationToken cancellationToken = default)
    {
        var result = await apiClientService.SendService<TokenModel>(HttpMethod.Post, $"{AppSettings.LoginBaseUrl}/Token", new {
            Username = username,
            Password = password
        }, cancellationToken);
        if (result is not null)
        {
            snackbar.Add("Login successfull", Severity.Info);
            await localStorageManager.SetAsync("authToken", result.Token);
            return result;
        }

        return null;
    }

    public async Task Logout()
    {
        await localStorageManager.RemoveAsync("authToken");
        snackbar.Add("Logout!", Severity.Info);
    }
}
