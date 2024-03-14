using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace NetCore.Identity.Model;

public class RoleClaimsModel
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    public IList<Claim> Claims { get; set; }
}
