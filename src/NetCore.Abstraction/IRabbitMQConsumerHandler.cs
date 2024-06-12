using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface IRabbitMQConsumerHandler<TData>
    where TData : class, new()
{
    void Handler(ReceivedMessageEventArgs<TData> args);
}
