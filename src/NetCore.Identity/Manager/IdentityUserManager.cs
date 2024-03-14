using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;
using NetCore.Abstraction.Model;
using NetCore.EntityFramework.Model;
using NetCore.ExceptionHandling;
using NetCore.Identity.Model;

namespace NetCore.Identity.Manager;

public class IdentityUserManager(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager, IOptions<JwtConfig> JwtConfig) : IIdentityUserManager
{
    public async Task<ResponseBase<IdentityResult>> CreateUser(RegisterUserModel model)
    {
        var userExists = await UserManager.FindByNameAsync(model.Username);
        if (userExists is not null)
            throw new UserLevelException("001", "User already exists!");

        var user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username
        };
        var result = await UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded && model.Roles != null && model.Roles.Count > 0)
            await UserManager.AddToRolesAsync(user, model.Roles);
        return !result.Succeeded
            ? throw new UserLevelException("003", "User creation failed! Please check user details and try again.")
            : new ResponseBase<IdentityResult>(result);
    }

    public async Task<ResponseBase> EditUserRoles(UpdateUserRolesModel model)
    {
        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new UserLevelException("002", "User not found!");
        var currentRoles = await UserManager.GetRolesAsync(user);

        // delete roles
        await UserManager.RemoveFromRolesAsync(user, currentRoles.Where(c => !model.Roles.Any(r => r.Equals(c))));

        //add roles
        await UserManager.AddToRolesAsync(user, model.Roles.Where(r => !currentRoles.Any(c => c.Equals(r))));

        return new ResponseBase(true, "000", "User updated roles successfully!");
    }

    public async Task<ResponseBase> EditUserClaims(UpdateUserClaimsModel model)
    {
        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new UserLevelException("002", "User not found!");
        var currentClaims = await UserManager.GetClaimsAsync(user);

        // delete roles
        await UserManager.RemoveClaimsAsync(user, currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))));

        //add roles
        await UserManager.AddClaimsAsync(user, model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))));

        return new ResponseBase(true, "000", "User updated claims successfully!");
    }

    public async Task<ResponseBase<UserModel>> GetUser(string username)
    {
        var user = await UserManager.FindByNameAsync(username) ?? throw new UserLevelException("002", "User not found!");
        var currentRoles = await UserManager.GetRolesAsync(user);
        return new ResponseBase<UserModel>(new UserModel()
        {
            Username = user.UserName,
            Email = user.Email,
            Roles = currentRoles,
            Id = user.Id
        });
    }

    public async Task<ResponseBase<IEnumerable<UserModel>>> GetAllUsers()
    {
        var users = UserManager.Users.ToList();

        var result = new List<UserModel>();
        foreach (var user in users)
            result.Add(new UserModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Roles = await UserManager.GetRolesAsync(user),
                Id = user.Id
            });

        return new ResponseBase<IEnumerable<UserModel>>(result);
    }

    public async Task<ResponseBase> RemoveUser(RemoveUserModel model)
    {
        var user = await UserManager.FindByNameAsync(model.Username) ?? throw new UserLevelException("002", "User not found!");
        await UserManager.DeleteAsync(user);

        return new ResponseBase(true, "000", "User removed successfully!");
    }
}
