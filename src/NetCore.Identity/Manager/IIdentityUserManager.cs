using Microsoft.AspNetCore.Identity;
using NetCore.Abstraction.Model;
using NetCore.Identity.Model;

namespace NetCore.Identity.Manager;
public interface IIdentityUserManager
{
    Task<ResponseBase<IdentityResult>> CreateUser(RegisterUserModel model);
    Task<ResponseBase> EditUserRoles(UpdateUserRolesModel model);
    Task<ResponseBase> EditUserClaims(UpdateUserClaimsModel model);
    Task<ResponseBase<UserModel>> GetUser(string username);
    Task<ResponseBase<IEnumerable<UserModel>>> GetAllUsers();
    Task<ResponseBase> RemoveUser(RemoveUserModel model);
}