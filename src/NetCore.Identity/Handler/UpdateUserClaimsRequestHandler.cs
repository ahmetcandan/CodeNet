using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class UpdateUserClaimsRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<UpdateUserClaimsModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(UpdateUserClaimsModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.EditUserClaims(request);
        }
    }
}
