using System.ComponentModel.DataAnnotations;

namespace NetCore.Identity.Model;

public class UpdateUserClaimsModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
