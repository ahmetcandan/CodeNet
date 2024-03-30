using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class GetRolesQueryHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<GetRoleQuery, ResponseBase<IEnumerable<RoleViewModel>>>
    {
        public async Task<ResponseBase<IEnumerable<RoleViewModel>>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.GetRoles(cancellationToken);
        }
    }
}
