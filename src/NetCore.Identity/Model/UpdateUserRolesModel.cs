using MediatR;
using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class UpdateUserRolesModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IList<string> Roles { get; set; }
}
