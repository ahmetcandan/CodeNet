using MediatR;
using Microsoft.AspNetCore.Identity;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class RegisterUserRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<RegisterUserModel, ResponseBase<IdentityResult>>
    {
        public async Task<ResponseBase<IdentityResult>> Handle(RegisterUserModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.CreateUser(request);
        }
    }
}
