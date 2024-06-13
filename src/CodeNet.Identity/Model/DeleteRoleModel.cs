using MediatR;
using CodeNet.Abstraction.Model;

namespace CodeNet.Identity.Model;

public class DeleteRoleModel : RoleModel, IRequest<ResponseBase>
{
}
