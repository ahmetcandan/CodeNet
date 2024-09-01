namespace CodeNet.Identity.Settings;

public class BaseIdentitySettings
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public double ExpiryTime { get; set; }
    public required string SecurityAlgorithm { get; set; }
}
