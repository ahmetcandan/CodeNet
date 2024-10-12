namespace CodeNet.Outbox.Models;

public class MessageModel
{
    public Guid Id { get; set; }
    public string MessageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string QueueName { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}
