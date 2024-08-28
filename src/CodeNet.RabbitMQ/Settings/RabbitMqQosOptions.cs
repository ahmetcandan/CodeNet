namespace CodeNet.RabbitMQ.Settings;

public class RabbitMqQosOptions
{
    public uint PrefetchSize { get; set; } = 0;
    public ushort PrefetchCount { get; set; } = 1;
    public bool Global { get; set; } = false;

    public override string ToString()
    {
        return $"PrefetchSize: {PrefetchSize}, PrefetchCount: {PrefetchCount}, Global: {Global}";
    }
}
