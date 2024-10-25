using CodeNet.Email.Builder;
using MongoDB.Bson.Serialization.Attributes;

namespace CodeNet.Email.Models;

internal class MailTemplate
{
    [BsonId]
    public string Code { get; set; }
    public TemplateBuilder Builder { get; set; }
    public bool IsBodyHtml { get; set; }
}
