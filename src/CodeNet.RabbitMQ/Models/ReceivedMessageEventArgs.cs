using Newtonsoft.Json;
using System.Text;

namespace CodeNet.RabbitMQ.Models;

public class ReceivedMessageEventArgs : EventArgs
{
    public required ReadOnlyMemory<byte> Data { get; set; }
    public string? MessageId { get; set; }
    public IDictionary<string, object>? Headers { get; set; }
    public string? Exchange { get; set; }
    public string? RoutingKey { get; set; }
    public string? ConsumerTag { get; set; }
    public ulong DeliveryTag { get; set; }
    public bool Redelivered { get; set; }

    public TModel? GetDataToModel<TModel>()
    {
        return JsonConvert.DeserializeObject<TModel>(GetDataToString());
    }

    public string GetDataToString()
    {
        return Encoding.UTF8.GetString(Data.ToArray());
    }
}
