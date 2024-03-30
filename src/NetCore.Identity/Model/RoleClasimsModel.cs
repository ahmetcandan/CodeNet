using MediatR;
using NetCore.Abstraction.Model;
using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class RoleClaimsModel : IRequest<ResponseBase>
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
