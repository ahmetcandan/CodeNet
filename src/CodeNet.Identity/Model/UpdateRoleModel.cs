using MediatR;
using CodeNet.Core.Models;

namespace CodeNet.Identity.Model;

public class UpdateRoleModel : RoleModel, IRequest<ResponseBase>
{
}
