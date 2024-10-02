namespace CodeNet.Core.Settings;

public class AuthenticationSettingsWithAsymmetricKey : AuthenticationSettings
{
    public required string PublicKeyPath { get; set; }
}
