using System.Net.Mail;

namespace CodeNet.Email.Settings;

public class SmtpOptions
{
    public required SmtpClient SmtpClient { get; set; }
    public required string EmailAddress { get; set; }
}

internal class MailOptions : SmtpOptions
{
    public bool HasMongoDB { get; set; }
}
