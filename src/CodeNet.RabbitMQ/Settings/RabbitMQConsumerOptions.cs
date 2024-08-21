using CodeNet.RabbitMQ.Services;

namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQConsumerOptions : BaseRabbitMQOptions
{
    public bool AutoAck { get; set; } = false;
    public bool NoLocal { get; set; } = false;
    public string ConsumerTag { get; set; } = "";

    public override string ToString()
    {
        return $"AutoAck: {AutoAck}, NoLocal: {NoLocal}, ConsumerTag: {ConsumerTag}, {base.ToString()}";
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