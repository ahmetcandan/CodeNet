using MediatR;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class RemoveUserRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<RemoveUserModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(RemoveUserModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.RemoveUser(request);
        }
    }
}
