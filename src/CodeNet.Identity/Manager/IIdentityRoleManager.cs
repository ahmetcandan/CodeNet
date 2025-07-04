using CodeNet.Core.Models;
using CodeNet.Identity.Models;

namespace CodeNet.Identity.Manager;

public interface IIdentityRoleManager
{
    Task<ResponseMessage> CreateRole(CreateRoleModel model);
    Task<ResponseMessage> EditRole(RoleModel model);
    Task<ResponseMessage> EditRoleClaims(RoleClaimsModel model);
    Task<ResponseMessage> DeleteRole(RoleModel model);
    Task<IEnumerable<RoleViewModel>> GetRoles(CancellationToken cancellationToken);
}