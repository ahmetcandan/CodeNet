namespace CodeNet.RabbitMQ.Models;

public class PublishModel(byte[] data, string messageId, IDictionary<string, object>? headers = null)
{
    public PublishModel(Guid id, byte[] data, string messageId, IDictionary<string, object>? headers = null) : this(data, messageId, headers)
    {
        Id = id;
    }

    public Guid Id { get; set; }
    public byte[] Data { get; set; } = data;
    public string MessageId { get; set; } = messageId;
    public IDictionary<string, object>? Headers { get; set; } = headers;
}
