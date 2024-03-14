using NetCore.Abstraction.Model;
using NetCore.Identity.Model;

namespace NetCore.Identity.Manager;

public interface IIdentityTokenManager
{
    Task<ResponseBase> GenerateToken(LoginModel model);
}