using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class UpdateUserClaimsModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IEnumerable<EditClaimsModel> Claims { get; set; }
}
