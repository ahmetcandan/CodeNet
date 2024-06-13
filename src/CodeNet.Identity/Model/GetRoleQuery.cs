using MediatR;
using CodeNet.Abstraction.Model;

namespace CodeNet.Identity.Model;

public class GetRoleQuery : IRequest<ResponseBase<IEnumerable<RoleViewModel>>>
{
}
