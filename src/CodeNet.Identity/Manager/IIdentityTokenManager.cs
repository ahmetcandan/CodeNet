using CodeNet.Core.Models;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Manager;

public interface IIdentityTokenManager
{
    Task<ResponseBase<TokenResponse>> GenerateToken(LoginModel model);
}