using CodeNet.Abstraction.Model;

namespace CodeNet.Abstraction;

public interface IRabbitMQConsumerHandler<TData>
    where TData : class, new()
{
    void Handler(ReceivedMessageEventArgs<TData> args);
}
