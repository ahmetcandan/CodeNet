namespace CodeNet.RabbitMQ.Models;

public class ReceivedMessageEventArgs<TModel> : EventArgs
    where TModel : class, new()
{
    public required TModel Data { get; set; }
    public string? MessageId { get; set; }
    public IDictionary<string, object>? Headers { get; set; }
    public string? Exchange { get; set; }
    public string? RoutingKey { get; set; }
    public string? ConsumerTag { get; set; }
    public ulong DeliveryTag { get; set; }
    public bool Redelivered { get; set; }
}
