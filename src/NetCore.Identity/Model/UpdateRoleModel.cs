using MediatR;
using NetCore.Abstraction.Model;

namespace NetCore.Identity.Model;

public class UpdateRoleModel : RoleModel, IRequest<ResponseBase>
{
}
