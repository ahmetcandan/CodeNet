namespace NetCore.Abstraction.Model;

public class RabbitMQSettings
{
    public required string HostName { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
