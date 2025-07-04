using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class UpdateUserRolesModel
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }

    public IEnumerable<string>? Roles { get; set; }
}
