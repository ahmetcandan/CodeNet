namespace CodeNet.Identity.Settings;

public class IdentitySettingsWithSymmetricKey
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
    public required string IssuerSigningKey { get; set; }
}
