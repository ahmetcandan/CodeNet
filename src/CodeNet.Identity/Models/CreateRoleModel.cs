using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class CreateRoleModel
{
    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    public string? NormalizedName { get; set; }
}
