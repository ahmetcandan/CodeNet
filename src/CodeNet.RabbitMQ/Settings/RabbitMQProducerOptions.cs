using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQProducerOptions : BaseRabbitMQOptions
{
    public string? Exchange { get; set; }
    public string? RoutingKey { get; set; }
}

public class RabbitMQProducerOptions<TProducerService> : RabbitMQProducerOptions
    where TProducerService : RabbitMQProducerService
{
}
