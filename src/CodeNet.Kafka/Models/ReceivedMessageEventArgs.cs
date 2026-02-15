using Confluent.Kafka;
namespace CodeNet.Kafka.Models;

public class ReceivedMessageEventArgs : ReceivedMessageEventArgs<string>
{
}

public class ReceivedMessageEventArgs<TValue> : ReceivedMessageEventArgs<Null, TValue>
{
}

public class ReceivedMessageEventArgs<TKey, TValue> : EventArgs
{
    public required Headers Headers { get; set; }
    public required TKey Key { get; set; }
    public required TValue Value { get; set; }
    public long Offset { get; set; }
    public int Partition { get; set; }
    public Timestamp Timestamp { get; set; }
}