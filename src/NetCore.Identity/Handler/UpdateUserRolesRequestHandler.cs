using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class UpdateUserRolesRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<UpdateUserRolesModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(UpdateUserRolesModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.EditUserRoles(request);
        }
    }
}
