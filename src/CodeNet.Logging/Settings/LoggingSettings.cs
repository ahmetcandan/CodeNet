namespace CodeNet.Logging.Settings;

public class LoggingSettings
{
    public required string ElasticsearchUrl { get; set; }
    public string IndexFormat { get; set; } = "appLog-{0:yyyy.MM}";
    public bool AutoRegisterTemplate { get; set; } = true;
    public required string Password { get; set; }
    public required string Username { get; set; }
}
