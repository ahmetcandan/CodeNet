using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class CreateRoleModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public string NormalizedName { get; set; }
}
