using Microsoft.AspNetCore.Identity;
using NetCore.Abstraction.Model;
using NetCore.Identity.Model;

namespace NetCore.Identity.Manager;
public interface IIdentityRoleManager
{
    Task<ResponseBase> CreateRole(CreateRoleModel model);
    Task<ResponseBase> EditRole(RoleModel model);
    Task<ResponseBase> EditRoleClaims(RoleClaimsModel model);
    Task<ResponseBase> DeleteRole(RoleModel model);
    Task<ResponseBase<IEnumerable<IdentityRole>>> GetRoles(CancellationToken cancellationToken);
}