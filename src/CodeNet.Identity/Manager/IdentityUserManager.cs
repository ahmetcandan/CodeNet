using Microsoft.AspNetCore.Identity;
using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Manager;

internal class IdentityUserManager(UserManager<IdentityUser> UserManager, RoleManager<IdentityRole> RoleManager) : IIdentityUserManager
{
    public async Task<ResponseBase<IdentityResult>> CreateUser(RegisterUserModel model)
    {
        var userExists = await UserManager.FindByNameAsync(model.Username);
        if (userExists is not null)
            throw new IdentityException("ID001", "User already exists!");

        var user = new IdentityUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded && model.Roles != null && model.Roles.Count > 0)
            await UserManager.AddToRolesAsync(user, model.Roles);
        return !result.Succeeded
            ? throw new IdentityException("ID003", "User creation failed! Please check user details and try again.")
            : new ResponseBase<IdentityResult>(result);
    }

    public async Task<ResponseBase> EditUserRoles(UpdateUserRolesModel model)
    {
        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        var currentRoles = await UserManager.GetRolesAsync(user);

        // delete roles
        await UserManager.RemoveFromRolesAsync(user, currentRoles.Where(c => !model.Roles.Any(r => r.Equals(c))));

        //add roles
        await UserManager.AddToRolesAsync(user, model.Roles.Where(r => !currentRoles.Any(c => c.Equals(r))));

        return new ResponseBase(true, "000", "User updated roles successfully!");
    }

    public async Task<ResponseBase> EditUserClaims(UpdateUserClaimsModel model)
    {
        IdentityException.ThrowIfNull(model?.Claims);

        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        var currentClaims = await UserManager.GetClaimsAsync(user);

        // delete roles
        await UserManager.RemoveClaimsAsync(user, currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))));

        //add roles
        await UserManager.AddClaimsAsync(user, model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))).Select(c => c.GetClaim()));

        return new ResponseBase(true, "000", "User updated claims successfully!");
    }

    public async Task<ResponseBase<UserModel>> GetUser(string username)
    {
        var user = await UserManager.FindByNameAsync(username) ?? throw new IdentityException("ID002", "User not found!");
        var claims = await UserManager.GetClaimsAsync(user);
        var roles = await UserManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            var role = await RoleManager.FindByNameAsync(roleName);
            var roleClaims = await RoleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
                claims.Add(new System.Security.Claims.Claim(claim.Type, claim.Value));
        }

        return new ResponseBase<UserModel>(new UserModel()
        {
            Username = user.UserName,
            Email = user.Email,
            Roles = roles,
            Claims = claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }),
            Id = user.Id
        });
    }

    public async Task<ResponseBase> RemoveUser(RemoveUserModel model)
    {
        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        await UserManager.DeleteAsync(user);

        return new ResponseBase(true, "000", "User removed successfully!");
    }
}
