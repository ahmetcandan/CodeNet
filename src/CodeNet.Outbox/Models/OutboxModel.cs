using CodeNet.MongoDB.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeNet.Outbox.Models;

[CollectionName("Outbox")]
internal class OutboxModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string MessageId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string QueueName { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}
