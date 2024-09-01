namespace CodeNet.Identity.Settings;

public class IdentitySettingsWithSymmetricKey : BaseIdentitySettings
{
    public required string IssuerSigningKey { get; set; }
}
