using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class UpdateRoleClaimsRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<RoleClaimsModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(RoleClaimsModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.EditRoleClaims(request);
        }
    }
}
