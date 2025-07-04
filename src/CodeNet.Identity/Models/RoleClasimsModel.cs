using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class RoleClaimsModel
{
    [Required(ErrorMessage = "Id is required")]
    public required string Id { get; set; }

    public IEnumerable<EditClaimsModel>? Claims { get; set; }
}
