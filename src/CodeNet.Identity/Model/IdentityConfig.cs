namespace CodeNet.Identity.Model;

public class IdentityConfig
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
    public required string PrivateKeyPath { get; set; }
}
