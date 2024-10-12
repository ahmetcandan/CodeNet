namespace CodeNet.RabbitMQ.Models;

public class PublishModel
{
    public PublishModel(Guid id, byte[] data, string messageId, IDictionary<string, object>? headers = null)
    {
        Id = id;
        Data = data;
        MessageId = messageId;
        Headers = headers;
    }

    public PublishModel(byte[] data, string messageId, IDictionary<string, object>? headers = null)
    {
        Data = data;
        MessageId = messageId;
        Headers = headers;
    }

    public Guid Id { get; set; }
    public byte[] Data { get; set; }
    public string MessageId { get; set; }
    public IDictionary<string, object>? Headers { get; set; }
}
