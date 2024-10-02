using System.ComponentModel.DataAnnotations;

namespace CodeNet.Identity.Settings;

public class LoginModelByToken
{
    [Required(ErrorMessage = "Token is required")]
    public string Token { get; set; }

    [Required(ErrorMessage = "Refresh Token is required")]
    public string RefreshToken { get; set; }
}
