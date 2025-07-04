using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class UpdateUserClaimsModel
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }

    public IEnumerable<EditClaimsModel>? Claims { get; set; }
}
