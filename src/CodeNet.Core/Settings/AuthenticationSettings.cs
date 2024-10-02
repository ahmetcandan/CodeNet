namespace CodeNet.Core.Settings;

public abstract class AuthenticationSettings
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
}
