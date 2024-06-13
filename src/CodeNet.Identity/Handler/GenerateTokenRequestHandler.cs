using MediatR;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Handler
{
    public class GenerateTokenRequestHandler(IIdentityTokenManager IdentityTokenManager) : IRequestHandler<LoginModel, ResponseBase<TokenResponse>>
    {
        public async Task<ResponseBase<TokenResponse>> Handle(LoginModel request, CancellationToken cancellationToken)
        {
            return await IdentityTokenManager.GenerateToken(request);
        }
    }
}
