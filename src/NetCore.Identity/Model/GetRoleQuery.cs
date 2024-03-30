using MediatR;
using NetCore.Abstraction.Model;

namespace NetCore.Identity.Model;

public class GetRoleQuery : IRequest<ResponseBase<IEnumerable<RoleViewModel>>>
{
}
