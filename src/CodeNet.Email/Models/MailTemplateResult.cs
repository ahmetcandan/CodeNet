using CodeNet.Email.Builder;

namespace CodeNet.Email.Models;

internal class MailTemplateResult
{
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
}
