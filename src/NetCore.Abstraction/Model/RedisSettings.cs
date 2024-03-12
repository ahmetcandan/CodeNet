namespace NetCore.Abstraction.Model;

public class RedisSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; }
    public int RetryTimeout { get; set; }
}
