namespace NetCore.Identity.Model;

public class JwtConfig
{
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
    public double ExpiryTime { get; set; }
}
