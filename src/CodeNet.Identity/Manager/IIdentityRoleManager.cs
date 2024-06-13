using CodeNet.Abstraction.Model;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Manager;

public interface IIdentityRoleManager
{
    Task<ResponseBase> CreateRole(CreateRoleModel model);
    Task<ResponseBase> EditRole(RoleModel model);
    Task<ResponseBase> EditRoleClaims(RoleClaimsModel model);
    Task<ResponseBase> DeleteRole(RoleModel model);
    Task<ResponseBase<IEnumerable<RoleViewModel>>> GetRoles(CancellationToken cancellationToken);
}