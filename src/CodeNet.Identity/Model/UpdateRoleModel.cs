using MediatR;
using CodeNet.Abstraction.Model;

namespace CodeNet.Identity.Model;

public class UpdateRoleModel : RoleModel, IRequest<ResponseBase>
{
}
