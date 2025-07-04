using CodeNet.Core.Models;
using CodeNet.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CodeNet.Identity.Manager;
public interface IIdentityUserManager
{
    Task<IdentityResult> CreateUser(RegisterUserModel model);
    Task<ResponseMessage> EditUserRoles(UpdateUserRolesModel model);
    Task<ResponseMessage> EditUserClaims(UpdateUserClaimsModel model);
    Task<UserModel> GetUser(string username);
    Task<ResponseMessage> RemoveUser(RemoveUserModel model);
}