using MediatR;
using CodeNet.Core.Models;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

namespace CodeNet.Identity.Api.Handler
{
    public class GetUserQueryHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<GetUserQuery, ResponseBase<UserModel>>
    {
        public async Task<ResponseBase<UserModel>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.GetUser(query.Username);
        }
    }
}
