namespace CodeNet.Email.Models;

public class TemplateModel
{
    public string Code { get; set; }
    public string Content { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Cc { get; set; } = string.Empty;
    public string Bcc { get; set; } = string.Empty;
    public bool IsBodyHtml { get; set; }
}
