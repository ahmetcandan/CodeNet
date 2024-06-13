using CodeNet.Abstraction.Model;

namespace CodeNet.Abstraction;

public interface IRabbitMQConsumerService<TData>
    where TData : class, new()
{
    void StartListening();
    event MessageReceived<TData>? ReceivedMessage;
}
