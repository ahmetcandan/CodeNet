using CodeNet.RabbitMQ.Models;

namespace CodeNet.RabbitMQ.Services;

public interface IRabbitMQConsumerHandler<TData>
    where TData : class, new()
{
    void Handler(ReceivedMessageEventArgs<TData> args);
}
