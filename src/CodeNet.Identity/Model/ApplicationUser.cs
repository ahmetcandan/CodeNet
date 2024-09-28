using Microsoft.AspNetCore.Identity;

namespace CodeNet.Identity.Model;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }
}
