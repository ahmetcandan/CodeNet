using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class CreateRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<CreateRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(CreateRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.CreateRole(request);
        }
    }
}
