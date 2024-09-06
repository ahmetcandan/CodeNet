namespace CodeNet.RabbitMQ.Settings;

public class RabbitMQExchangeOptions
{
    public string Name { get; set; }
    public string Type { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Type: {Type}";
    }
}
