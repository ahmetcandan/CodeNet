using Newtonsoft.Json;
using System.Text;

namespace CodeNet.RabbitMQ.Models;

public class ReceivedMessageEventArgs : EventArgs
{
    public required ReadOnlyMemory<byte> Data { get; set; }
    public string? MessageId { get; internal set; }
    public IDictionary<string, object>? Headers { get; internal set; }
    public string? Exchange { get; internal set; }
    public string? RoutingKey { get; internal set; }
    public string? ConsumerTag { get; internal set; }
    public ulong DeliveryTag { get; internal set; }
    public bool Redelivered { get; internal set; }
    public DateTimeOffset? Timestamp { get; internal set; }
    public string? AppId { get; internal set; }
    public string? ClusterId { get; internal set; }
    public DeliveredMode DeliveryMode { get; internal set; }
    public byte? Priority { get; internal set; }
    public string? CorrelationId { get; internal set; }
    public string? Type { get; internal set; }

    public TModel? GetDataToModel<TModel>()
    {
        return JsonConvert.DeserializeObject<TModel>(GetDataToString());
    }

    public string GetDataToString()
    {
        return Encoding.UTF8.GetString(Data.ToArray());
    }
}
