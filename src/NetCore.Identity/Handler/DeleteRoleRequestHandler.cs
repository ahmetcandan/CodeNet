using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class DeleteRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<DeleteRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(DeleteRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.DeleteRole(request);
        }
    }
}
