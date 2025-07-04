using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Models;

public class LoginModel
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
