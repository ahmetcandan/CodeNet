using CodeNet.Core.Models;
using CodeNet.Identity.Exception;
using CodeNet.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CodeNet.Identity.Manager;

internal class IdentityRoleManager<TRole, TKey>(RoleManager<TRole> roleManager) : IIdentityRoleManager
    where TRole : IdentityRole<TKey>, new()
    where TKey : IEquatable<TKey>
{
    public async Task<ResponseMessage> CreateRole(CreateRoleModel model)
    {
        var roleExists = await roleManager.FindByNameAsync(model.Name);
        if (roleExists is not null)
            throw new IdentityException(ExceptionMessages.RoleAlreadyExists);

        TRole role = new()
        {
            Name = model.Name,
            NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName
        };
        var result = await roleManager.CreateAsync(role);

        return !result.Succeeded
            ? throw new IdentityException(ExceptionMessages.RoleCreationFailed)
            : new ResponseMessage("000", "Role created successfully!");
    }

    public async Task<ResponseMessage> EditRole(RoleModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id) ?? throw new IdentityException(ExceptionMessages.RoleNotFound);
        role.Name = model.Name;
        role.NormalizedName = string.IsNullOrEmpty(model.NormalizedName)
            ? model.Name.Replace(" ", "").ToUpper()
            : model.NormalizedName;
        var result = await roleManager.UpdateAsync(role);

        return !result.Succeeded
            ? throw new IdentityException(ExceptionMessages.RoleUpdateFailed)
            : new ResponseMessage("000", "Role updated successfully!");
    }

    public async Task<ResponseMessage> EditRoleClaims(RoleClaimsModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id) ?? throw new IdentityException(ExceptionMessages.RoleNotFound);
        var currentClaims = await roleManager.GetClaimsAsync(role);

        // delete roles
        foreach (var claim in currentClaims.Where(c => model.Claims?.Any(r => r.Type.Equals(c.Type)) is not true))
            await roleManager.RemoveClaimAsync(role, claim);

        //add roles
        if (model.Claims is not null)
            foreach (var claim in model.Claims.Where(r => currentClaims?.Any(c => c.Type.Equals(r.Type)) is not true))
                await roleManager.AddClaimAsync(role, claim.GetClaim());

        return new ResponseMessage("000", "Role claims updated successfully!");
    }

    public async Task<ResponseMessage> DeleteRole(RoleModel model)
    {
        var role = await roleManager.FindByIdAsync(model.Id) ?? throw new IdentityException(ExceptionMessages.RoleNotFound);
        var result = await roleManager.DeleteAsync(role);

        return !result.Succeeded
            ? throw new IdentityException(ExceptionMessages.RoleDeleteFailed)
            : new ResponseMessage("000", "Role deleted successfully!");
    }

    public async Task<IEnumerable<RoleViewModel>> GetRoles(CancellationToken cancellationToken) => await GetRolesWithClaims(cancellationToken);

    private async Task<IEnumerable<RoleViewModel>> GetRolesWithClaims(CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);
        var list = new List<RoleViewModel>(roles.Count);
        foreach (var role in roles)
            list.Add(new()
            {
                Id = role.Id.ToString() ?? string.Empty,
                Name = role.Name ?? string.Empty,
                NormalizedName = role.NormalizedName,
                Claims = (await roleManager.GetClaimsAsync(role)).Select(c => new ClaimModel(c.Type, c.Value))
            });

        return list;
    }
}
