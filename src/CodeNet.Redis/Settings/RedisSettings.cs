namespace CodeNet.Redis.Settings;

public class RedisSettings
{
    public required string Hostname { get; set; }
    public int Port { get; set; }

    private string? _instanceName;
    public string InstanceName
    {
        get
        {
            return string.IsNullOrEmpty(_instanceName) ? "master" : (_instanceName ?? string.Empty);
        }
        set
        {
            _instanceName = value;
        }
    }
}
