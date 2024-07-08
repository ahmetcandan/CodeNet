using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class UpdateUserRolesModel
{
    [Required(ErrorMessage = "User Name is required")]
    public string Username { get; set; }

    public IList<string> Roles { get; set; }
}
