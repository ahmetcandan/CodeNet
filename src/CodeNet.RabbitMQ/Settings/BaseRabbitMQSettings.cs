namespace CodeNet.RabbitMQ.Settings;

public abstract class BaseRabbitMQSettings
{
    public required string HostName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? ClientProvidedName { get; set; }
    public uint? MaxMessageSize { get; set; }
    public int? Port { get; set; }
    public TimeSpan? SocketReadTimeout { get; set; }
    public string? Queue { get; set; }
    public bool Durable { get; set; }
    public bool Exclusive { get; set; }
    public bool AutoDelete { get; set; }
}
