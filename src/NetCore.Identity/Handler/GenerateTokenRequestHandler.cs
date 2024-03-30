using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Cache;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class GenerateTokenRequestHandler(IIdentityTokenManager IdentityTokenManager) : IRequestHandler<LoginModel, ResponseBase<TokenResponse>>
    {
        [DistributedLock]
        public async Task<ResponseBase<TokenResponse>> Handle(LoginModel request, CancellationToken cancellationToken)
        {
            return await IdentityTokenManager.GenerateToken(request);
        }
    }
}
