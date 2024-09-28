using CodeNet.Core.Models;
using CodeNet.Identity.Settings;

namespace CodeNet.Identity.Manager;

public interface IIdentityTokenManager
{
    Task<TokenResponse> GenerateToken(LoginModel model, bool generateRefreshToken = true);
    Task<TokenResponse> GenerateToken(string token, string refreshToken);
    Task<ResponseMessage> RemoveRefreshToken();
}