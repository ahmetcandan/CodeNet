using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using NetCore.Abstraction.Model;
using NetCore.EntityFramework.Model;
using NetCore.ExceptionHandling;
using NetCore.Identity.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCore.Identity.Manager;

public class IdentityRoleManager(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager, IOptions<JwtConfig> JwtConfig) : IIdentityRoleManager
{
    public async Task<ResponseBase> CreateRole(CreateRoleModel model)
    {
        var roleExists = await RoleManager.FindByNameAsync(model.Name);
        if (roleExists is not null)
            throw new UserLevelException("011", "Role already exists!");

        IdentityRole role = new()
        {
            Name = model.Name,
            NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName
        };
        var result = await RoleManager.CreateAsync(role);

        return !result.Succeeded
            ? throw new UserLevelException("013", "Role creation failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role created successfully!");
    }

    public async Task<ResponseBase> EditRole(RoleModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new UserLevelException("012", "Role not found!");
        role.Name = model.Name;
        role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName;
        var result = await RoleManager.UpdateAsync(role);

        return !result.Succeeded
            ? throw new UserLevelException("014", "Role update failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role updated successfully!");
    }

    public async Task<ResponseBase> EditRoleClaims(RoleClaimsModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new UserLevelException("012", "Role not found!");
        var currentClaims = await RoleManager.GetClaimsAsync(role);

        // delete roles
        foreach (var claim in currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))))
            await RoleManager.RemoveClaimAsync(role, claim);

        //add roles
        foreach (var claim in model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))))
            await RoleManager.AddClaimAsync(role, claim);

        return new ResponseBase(true, "000", "Role claims updated successfully!");
    }

    public async Task<ResponseBase> DeleteRole(RoleModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new UserLevelException("012", "Role not found!");
        var result = await RoleManager.DeleteAsync(role);

        return !result.Succeeded
            ? throw new UserLevelException("015", "Role delete failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role deleted successfully!");
    }

    public async Task<ResponseBase<IEnumerable<IdentityRole>>> GetRoles(CancellationToken cancellationToken)
    {
        return new ResponseBase<IEnumerable<IdentityRole>>(await RoleManager.Roles.ToListAsync(cancellationToken));
    }
}
