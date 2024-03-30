using MediatR;
using NetCore.Abstraction.Model;

namespace NetCore.Identity.Model;

public class DeleteRoleModel : RoleModel, IRequest<ResponseBase>
{
}
