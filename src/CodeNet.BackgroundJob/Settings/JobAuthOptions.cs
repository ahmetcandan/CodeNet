namespace CodeNet.BackgroundJob.Settings;

internal class JobAuthOptions
{
    public AuthenticationType? AuthenticationType { get; set; }
    public JobJwtAuthOptions? JwtAuthOptions { get; set; }
    public JobBasicAuthOptions? BasicAuthOptions { get; set; }
}

internal class JobJwtAuthOptions
{
    public string Roles { get; set; }
    public string Users { get; set; }
}

internal class JobBasicAuthOptions
{
    public Dictionary<string, string> UserPass { get; set; }
}
