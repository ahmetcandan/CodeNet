using MediatR;
using CodeNet.Core.Models;

namespace CodeNet.Identity.Model;

public class GetRoleQuery : IRequest<ResponseBase<IEnumerable<RoleViewModel>>>
{
}
