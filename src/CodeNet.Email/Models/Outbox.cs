using MongoDB.Bson.Serialization.Attributes;
using System.Net.Mail;

namespace CodeNet.Email.Models;

public partial class Outbox
{
    [BsonId]
    public Guid Id { get; set; }
    public DateTime SendDate { get; set; }
    public string From { get; set; }
    public MailAddressCollection To { get; set; }
    public MailAddressCollection Cc { get; set; }
    public MailAddressCollection Bcc { get; set; }
    public string TemplateCode { get; set; }
    public string Body { get; set; }
}
