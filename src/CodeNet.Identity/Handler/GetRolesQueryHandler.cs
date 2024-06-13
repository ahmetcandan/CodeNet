using MediatR;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Handler
{
    public class GetRolesQueryHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<GetRoleQuery, ResponseBase<IEnumerable<RoleViewModel>>>
    {
        public async Task<ResponseBase<IEnumerable<RoleViewModel>>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.GetRoles(cancellationToken);
        }
    }
}
