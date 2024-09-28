namespace CodeNet.Identity.Settings;

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public DateTime CreatedDate { get; set; }
    public IEnumerable<ClaimResponse> Claims { get; set; } = [];
}
