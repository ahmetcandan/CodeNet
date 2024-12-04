namespace CodeNet.EventBus.EventDefinitions;

public class MessageConsumedArguments(byte[] message)
{
    public MessageConsumedArguments(byte[] message, string consumerGroup) : this(message)
    {
        ConsumerGroup = consumerGroup;
    }

    public byte[] Message { get; set; } = message;
    public string ConsumerGroup { get; set; } = string.Empty;
    public DateTime Date { get; } = DateTime.Now;
}
