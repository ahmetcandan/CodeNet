using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class RoleClaimsModel
{
    [Required(ErrorMessage = "Id is required")]
    public string Id { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
