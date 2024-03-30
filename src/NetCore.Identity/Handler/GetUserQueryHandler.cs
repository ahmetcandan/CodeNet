using MediatR;
using NetCore.Abstraction.Model;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

namespace NetCore.Identity.Handler
{
    public class GetUserQueryHandler(IIdentityUserManager IdentityUserManager) : IRequestHandler<GetUserQuery, ResponseBase<UserModel>>
    {
        public async Task<ResponseBase<UserModel>> Handle(GetUserQuery query, CancellationToken cancellationToken)
        {
            return await IdentityUserManager.GetUser(query.Username);
        }
    }
}
