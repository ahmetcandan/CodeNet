using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface IRabbitMQConsumerService<TData>
    where TData : class, new()
{
    void StartListening();
    event MessageReceived<TData>? ReceivedMessage;
}
