using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Model;
using System.Data;

namespace CodeNet.Identity.Manager;

internal class IdentityRoleManager(RoleManager<IdentityRole> RoleManager) : IIdentityRoleManager
{
    public async Task<ResponseBase> CreateRole(CreateRoleModel model)
    {
        var roleExists = await RoleManager.FindByNameAsync(model.Name);
        if (roleExists is not null)
            throw new IdentityException("ID011", "Role already exists!");

        IdentityRole role = new()
        {
            Name = model.Name,
            NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName
        };
        var result = await RoleManager.CreateAsync(role);

        return !result.Succeeded
            ? throw new IdentityException("ID013", "Role creation failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role created successfully!");
    }

    public async Task<ResponseBase> EditRole(RoleModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new IdentityException("ID012", "Role not found!");
        role.Name = model.Name;
        role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName;
        var result = await RoleManager.UpdateAsync(role);

        return !result.Succeeded
            ? throw new IdentityException("ID014", "Role update failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role updated successfully!");
    }

    public async Task<ResponseBase> EditRoleClaims(RoleClaimsModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new IdentityException("ID012", "Role not found!");
        var currentClaims = await RoleManager.GetClaimsAsync(role);

        // delete roles
        foreach (var claim in currentClaims.Where(c => !model.Claims.Any(r => r.Type.Equals(c.Type))))
            await RoleManager.RemoveClaimAsync(role, claim);

        //add roles
        foreach (var claim in model.Claims.Where(r => !currentClaims.Any(c => c.Type.Equals(r.Type))))
            await RoleManager.AddClaimAsync(role, claim.GetClaim());

        return new ResponseBase(true, "000", "Role claims updated successfully!");
    }

    public async Task<ResponseBase> DeleteRole(RoleModel model)
    {
        var role = await RoleManager.FindByIdAsync(model.Id) ?? throw new IdentityException("ID012", "Role not found!");
        var result = await RoleManager.DeleteAsync(role);

        return !result.Succeeded
            ? throw new IdentityException("ID015", "Role delete failed! Please check role details and try again.")
            : new ResponseBase(true, "000", "Role deleted successfully!");
    }

    public async Task<ResponseBase<IEnumerable<RoleViewModel>>> GetRoles(CancellationToken cancellationToken)
    {
        return new ResponseBase<IEnumerable<RoleViewModel>>(await GetRolesWithClaims(cancellationToken));
    }

    private async Task<IEnumerable<RoleViewModel>> GetRolesWithClaims(CancellationToken cancellationToken)
    {
        var roles = await RoleManager.Roles.ToListAsync(cancellationToken);
        var list = new List<RoleViewModel>(roles.Count);
        foreach (var role in roles)
            list.Add(new()
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                Claims = (await RoleManager.GetClaimsAsync(role)).Select(c => new ClaimModel(c.Type, c.Value))
            });

        return list;
    }
}
