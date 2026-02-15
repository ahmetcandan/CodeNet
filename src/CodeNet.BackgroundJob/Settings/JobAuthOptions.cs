namespace CodeNet.BackgroundJob.Settings;

internal class JobAuthOptions
{
    public AuthenticationType? AuthenticationType { get; set; }
    public JobJwtAuthOptions? JwtAuthOptions { get; set; }
    public JobBasicAuthOptions? BasicAuthOptions { get; set; }
}

internal class JobJwtAuthOptions
{
    public required string Roles { get; set; }
    public required string Users { get; set; }
}

internal class JobBasicAuthOptions
{
    public required Dictionary<string, string> UserPass { get; set; }
}
