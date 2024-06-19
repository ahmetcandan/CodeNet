using MediatR;
using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class UpdateRoleRequestHandler(IIdentityRoleManager IdentityRoleManager) : IRequestHandler<UpdateRoleModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(UpdateRoleModel request, CancellationToken cancellationToken)
        {
            return await IdentityRoleManager.EditRole(request);
        }
    }
}
