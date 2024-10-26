using CodeNet.Email.Builder;

namespace CodeNet.Email.Models;

internal class MailTemplateResult
{
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
    public string Bcc { get; set; }
    public string Cc { get; set; }
    public string To { get; set; }
    public string From { get; set; }
}
