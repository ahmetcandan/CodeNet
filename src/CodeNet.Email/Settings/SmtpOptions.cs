using System.Net.Mail;

namespace CodeNet.Email.Settings;

public class SmtpOptions
{
    public SmtpClient SmtpClient { get; set; }
    public string EmailAddress { get; set; }
}

internal class MailOptions : SmtpOptions
{
    public bool HasMongoDB { get; set; }
}
