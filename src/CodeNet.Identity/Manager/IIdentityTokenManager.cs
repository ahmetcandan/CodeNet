using CodeNet.Identity.Settings;

namespace CodeNet.Identity.Manager;

public interface IIdentityTokenManager
{
    Task<TokenResponse> GenerateToken(LoginModel model);
}