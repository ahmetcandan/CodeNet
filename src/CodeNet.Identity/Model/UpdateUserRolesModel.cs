using MediatR;
using CodeNet.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Model;

public class UpdateUserRolesModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IList<string> Roles { get; set; }
}
