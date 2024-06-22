using MediatR;
using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class UpdateUserClaimsRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<UpdateUserClaimsModel, ResponseBase>
    {
        public async Task<ResponseBase> Handle(UpdateUserClaimsModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.EditUserClaims(request);
        }
    }
}
