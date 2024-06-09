using NetCore.Abstraction.Model;

namespace NetCore.Abstraction;

public interface IRabbitMQService<TData>
    where TData : class, new()
{
    public bool Post(TData data);
    public bool Post(TData data, string messageId);
    public bool Post(TData data, string messageId, IDictionary<string, object> headers);
    void ListenConnection();
    event MessageReceived<TData>? ReceivedMessage;
}
