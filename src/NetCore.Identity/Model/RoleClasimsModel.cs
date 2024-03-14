using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class RoleClaimsModel
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
