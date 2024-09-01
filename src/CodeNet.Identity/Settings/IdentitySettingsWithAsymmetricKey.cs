namespace CodeNet.Identity.Settings;

public class IdentitySettingsWithAsymmetricKey : BaseIdentitySettings
{
    public required string PrivateKeyPath { get; set; }
}
