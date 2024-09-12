using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQConsumerOptions : BaseRabbitMQOptions
{
    public bool AsyncConsumer { get; set; } = false;
    public bool AutoAck { get; set; } = false;
    public bool NoLocal { get; set; } = false;
    public string ConsumerTag { get; set; } = "";
    public RabbitMQQosOptions? Qos { get; set; }
    public IDictionary<string, object>? ConsumerArguments { get; set; } = null;

    public override string ToString()
    {
        return $"{base.ToString()}, AutoAck: {AutoAck}, NoLocal: {NoLocal}, ConsumerTag: {ConsumerTag}{(Qos is not null ? $", Qos: {{ {Qos} }}" : "")}";
    }
}

public class RabbitMQConsumerOptions<TConsumerService> : RabbitMQConsumerOptions
    where TConsumerService : RabbitMQConsumerService
{
    public override string ToString()
    {
        return base.ToString();
    }
}
