using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQProducerOptions : BaseRabbitMQOptions
{
    public string? RoutingKey { get; set; }
    public bool? Mandatory { get; set; } = false;
    public RabbitMQExchangeOptions? Exchange { get; set; }

    public override string ToString()
    {
        return $"{base.ToString()}, Exchange: {Exchange}, RoutingKey: {RoutingKey}, Mandatory: {Mandatory}{(Exchange is not null ? $", Exchange: {{{Exchange}}}" : "")}";
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
