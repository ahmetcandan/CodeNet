using MediatR;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Handler
{
    public class DeleteRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<DeleteRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(DeleteRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.DeleteRole(request);
        }
    }
}
