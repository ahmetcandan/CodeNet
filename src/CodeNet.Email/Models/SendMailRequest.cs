using System.Net.Mail;

namespace CodeNet.Email.Models;

public class SendMailRequest
{
    public MailAddressCollection To { get; set; }
    public MailAddressCollection Cc { get; set; }
    public MailAddressCollection Bcc { get; set; }
    public string TemplateCode { get; set; }
    public object Params { get; set; }
    public string Subject { get; set; }
}
