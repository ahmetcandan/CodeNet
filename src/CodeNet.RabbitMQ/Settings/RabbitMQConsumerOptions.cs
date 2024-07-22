using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQConsumerOptions : BaseRabbitMQOptions
{
    public bool AutoAck { get; set; }
}

public class RabbitMQConsumerOptions<TConsumerService> : RabbitMQConsumerOptions
    where TConsumerService : RabbitMQConsumerService
{
}