using RabbitMQ.Client;

namespace CodeNet.RabbitMQ.Settings;

public abstract class BaseRabbitMQOptions
{
    public string Queue { get; set; } = "";
    public string Exchange { get; set; } = "";
    public string RoutingKey { get; set; } = "";
    public string ExchangeType { get; set; } = "";
    public bool Durable { get; set; } = false;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
    public IDictionary<string, object>? Arguments { get; set; } = null;
    public IDictionary<string, object>? QueueBindArguments { get; set; } = null;
    public IDictionary<string, object>? ExchangeArguments { get; set; } = null;
    public bool DeclareQueue { get; set; } = true;
    public bool DeclareExchange { get; set; } = false;
    public bool QueueBind { get; set; } = false;
    public required ConnectionFactory ConnectionFactory { get; set; }

    public override string ToString() => $"Queue: {Queue}, Exchange: {Exchange}, ExchangeType: {ExchangeType}, RoutingKey: {RoutingKey}, Durable: {Durable}, Exclusive: {Exclusive}, AutoDelete: {AutoDelete}, DeclareQueue: {DeclareQueue}, DeclareExchange: {DeclareExchange}, QueueBind: {QueueBind}";
}
