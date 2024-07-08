namespace CodeNet.Identity.Settings;

public class IdentitySettingsWithAsymmetricKey
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
    public required string PrivateKeyPath { get; set; }
}
