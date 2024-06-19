using Microsoft.AspNetCore.Identity;
using CodeNet.Core.Models;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Manager;
public interface IIdentityUserManager
{
    Task<ResponseBase<IdentityResult>> CreateUser(RegisterUserModel model);
    Task<ResponseBase> EditUserRoles(UpdateUserRolesModel model);
    Task<ResponseBase> EditUserClaims(UpdateUserClaimsModel model);
    Task<ResponseBase<UserModel>> GetUser(string username);
    Task<ResponseBase> RemoveUser(RemoveUserModel model);
}