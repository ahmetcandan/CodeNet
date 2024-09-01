using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQProducerOptions : BaseRabbitMQOptions
{
    public string? RoutingKey { get; set; }
    public bool? Mandatory { get; set; }

    public override string ToString()
    {
        return $"Exchange: {Exchange}, RoutingKey: {RoutingKey}, Mandatory: {Mandatory}, {base.ToString()}";
    }
}

public class RabbitMQProducerOptions<TProducerService> : RabbitMQProducerOptions
    where TProducerService : RabbitMQProducerService
{
    public override string ToString()
    {
        return base.ToString();
    }
}
