using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class RoleModel
{
    [Required(ErrorMessage = "Id is required")]
    public required string Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required string Name { get; set; }

    public string? NormalizedName { get; set; }
}
