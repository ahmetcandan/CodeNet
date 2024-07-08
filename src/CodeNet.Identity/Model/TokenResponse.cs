namespace CodeNet.Identity.Settings;

public class TokenResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public DateTime CreatedDate { get; set; }
    public IEnumerable<ClaimResponse> Claims { get; set; }
}
