namespace CodeNet.Core.Settings;

public class AuthenticationSettingsWithSymmetricKey
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public required string IssuerSigningKey { get; set; }
}
