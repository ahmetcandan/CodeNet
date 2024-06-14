using MediatR;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class CreateRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<CreateRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(CreateRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.CreateRole(request);
        }
    }
}
