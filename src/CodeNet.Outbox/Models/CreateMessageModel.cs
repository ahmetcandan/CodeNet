namespace CodeNet.Outbox.Models;

public class CreateMessageModel
{
    public string QueueName { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}
