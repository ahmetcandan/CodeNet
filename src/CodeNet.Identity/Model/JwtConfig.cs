namespace CodeNet.Identity.Model;

public class JwtConfig
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
}
