using MediatR;
using Microsoft.AspNetCore.Identity;
using CodeNet.Abstraction.Model;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class RegisterUserRequestHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<RegisterUserModel, ResponseBase<IdentityResult>>
    {
        public async Task<ResponseBase<IdentityResult>> Handle(RegisterUserModel request, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.CreateUser(request);
        }
    }
}
