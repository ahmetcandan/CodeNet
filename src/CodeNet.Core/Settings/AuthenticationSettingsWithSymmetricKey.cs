namespace CodeNet.Core.Settings;

public class AuthenticationSettingsWithSymmetricKey : AuthenticationSettings
{
    public required string IssuerSigningKey { get; set; }
}
