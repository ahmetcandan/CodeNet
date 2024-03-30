using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class UpdateRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<UpdateRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(UpdateRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.EditRole(request);
        }
    }
}
