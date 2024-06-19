using CodeNet.RabbitMQ.Models;

namespace CodeNet.RabbitMQ.Services;

public interface IRabbitMQConsumerService<TData>
    where TData : class, new()
{
    void StartListening();
    event MessageReceived<TData>? ReceivedMessage;
}
