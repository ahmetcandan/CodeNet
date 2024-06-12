using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface IRabbitMQProducerService<TData>
    where TData : class, new()
{
    bool Publish(TData data);
    bool Publish(TData data, string messageId);
    bool Publish(TData data, string messageId, IDictionary<string, object> headers);
}
