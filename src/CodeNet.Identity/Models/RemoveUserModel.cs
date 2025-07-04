using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class RemoveUserModel
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }
}
