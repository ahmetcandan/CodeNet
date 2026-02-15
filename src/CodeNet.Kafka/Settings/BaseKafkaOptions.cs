namespace CodeNet.Kafka.Settings;

public abstract class BaseKafkaOptions
{
    public string? Topic { get; set; }

    public override string ToString()
    {
        return $"Topic: {Topic}";
    }
}
