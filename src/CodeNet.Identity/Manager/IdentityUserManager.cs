using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Settings;
using Microsoft.AspNetCore.Identity;

namespace CodeNet.Identity.Manager;

internal class IdentityUserManager(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : IIdentityUserManager
{
    public async Task<IdentityResult> CreateUser(RegisterUserModel model)
    {
        var userExists = await userManager.FindByNameAsync(model.Username);
        if (userExists is not null)
            throw new IdentityException("ID001", "User already exists!");

        var user = new IdentityUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded && model.Roles != null && model.Roles.Count > 0)
            await userManager.AddToRolesAsync(user, model.Roles);
        return !result.Succeeded
            ? throw new IdentityException("ID003", "User creation failed! Please check user details and try again.")
            : result;
    }

    public async Task<ResponseMessage> EditUserRoles(UpdateUserRolesModel model)
    {
        var user = await userManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        var currentRoles = await userManager.GetRolesAsync(user);

        // delete roles
        await userManager.RemoveFromRolesAsync(user, currentRoles.Where(c => !model.Roles.Any(r => r.Equals(c))));

        //add roles
        await userManager.AddToRolesAsync(user, model.Roles.Where(r => !currentRoles.Any(c => c.Equals(r))));

        return new ResponseMessage("000", "User updated roles successfully!");
    }

    public async Task<ResponseMessage> EditUserClaims(UpdateUserClaimsModel model)
    {
        IdentityException.ThrowIfNull(model?.Claims);

        var user = await userManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        var currentClaims = await userManager.GetClaimsAsync(user);

        // delete roles
        await userManager.RemoveClaimsAsync(user, currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))));

        //add roles
        await userManager.AddClaimsAsync(user, model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))).Select(c => c.GetClaim()));

        return new ResponseMessage("000", "User updated claims successfully!");
    }

    public async Task<UserModel> GetUser(string username)
    {
        var user = await userManager.FindByNameAsync(username) ?? throw new IdentityException("ID002", "User not found!");
        var claims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            var roleClaims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in roleClaims)
                claims.Add(new System.Security.Claims.Claim(claim.Type, claim.Value));
        }

        return new UserModel()
        {
            Username = user.UserName,
            Email = user.Email,
            Roles = roles,
            Claims = claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }),
            Id = user.Id
        };
    }

    public async Task<ResponseMessage> RemoveUser(RemoveUserModel model)
    {
        var user = await userManager.FindByNameAsync(model.Username) ?? throw new IdentityException("ID002", "User not found!");
        await userManager.DeleteAsync(user);

        return new ResponseMessage("000", "User removed successfully!");
    }
}
