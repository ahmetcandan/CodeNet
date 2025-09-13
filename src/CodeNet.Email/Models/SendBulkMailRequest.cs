using System.Net.Mail;

namespace CodeNet.Email.Models;

public class SendBulkMailRequest
{
    public required string TemplateCode { get; set; }
    public required string Subject { get; set; }
    public required ICollection<SendBulkMail> MailInfos { get; set; }
}

public class SendBulkMail
{
    public object? Param { get; set; }
    public MailAddressCollection To { get; set; } = [];
    public MailAddressCollection Cc { get; set; } = [];
    public MailAddressCollection Bcc { get; set; } = [];
}