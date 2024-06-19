namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQProducerSettings : BaseRabbitMQSettings
{
    public string? Exchange { get; set; }
    public string? RoutingKey { get; set; }
}
