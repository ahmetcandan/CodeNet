using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class RemoveUserRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<RemoveUserModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(RemoveUserModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.RemoveUser(request);
        }
    }
}
