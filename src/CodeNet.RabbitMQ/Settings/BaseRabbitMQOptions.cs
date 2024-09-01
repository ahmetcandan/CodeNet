using RabbitMQ.Client;

namespace CodeNet.RabbitMQ.Settings;

public abstract class BaseRabbitMQOptions
{
    public string Queue { get; set; } = "";
    public bool Durable { get; set; } = false;
    public bool Exclusive { get; set; } = false;
    public bool AutoDelete { get; set; } = false;
    public string Exchange { get; set; } = "";
    public string Type { get; set; } = "";
    public IDictionary<string, object>? Arguments { get; set; } = null;
    public bool DeclareQueue { get; set; } = true;
    public bool DeclareExchange { get; set; } = false;
    public ConnectionFactory ConnectionFactory { get; set; }

    public override string ToString()
    {
        return $"Queue: {Queue}, Durable: {Durable}, Exclusive: {Exclusive}, AutoDelete: {AutoDelete}, Exchange: {Exchange}, Type: {Type}, DeclareQueue: {DeclareQueue}, DeclareExchange: {DeclareExchange}";
    }
}
