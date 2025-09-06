using CodeNet.Messaging.Builder;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeNet.Email.Models;

public partial class MailTemplate
{
    [BsonId]
    public required string Code { get; set; }
    public TemplateBuilder? Builder { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Cc { get; set; } = string.Empty;
    public string Bcc { get; set; } = string.Empty;
    public bool IsBodyHtml { get; set; }
}
