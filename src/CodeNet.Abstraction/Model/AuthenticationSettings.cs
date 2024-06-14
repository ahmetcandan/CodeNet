namespace CodeNet.Abstraction.Model;

public class AuthenticationSettings
{
    public required string ValidAudience { get; set; }
    public required string ValidIssuer { get; set; }
    public required string PublicKeyPath { get; set; }
}
