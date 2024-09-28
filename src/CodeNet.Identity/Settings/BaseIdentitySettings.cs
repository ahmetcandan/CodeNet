namespace CodeNet.Identity.Settings;

public abstract class BaseIdentitySettings
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
    public double RefreshTokenExpiryTime { get; set; }
    public required string SecurityAlgorithm { get; set; }
}
