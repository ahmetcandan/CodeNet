using System.Net.Mail;
using CodeNet.Email.Models;
using Microsoft.Extensions.Options;
using CodeNet.Email.Settings;
using CodeNet.Email.Repositories;

namespace CodeNet.Email.Services;

internal class EmailService(MailTemplateRepositories templateRepositories, IOptions<SmtpOptions> options) : IEmailService
{
    public async Task SendMail(SendMailRequest request, CancellationToken cancellationToken)
    {
        var mailTemplate = await GetTemplate(request.TemplateCode, request.Params, cancellationToken);

        MailMessage mailMessage = new()
        {
            From = new MailAddress(options.Value.EmailAddress),
            Subject = request.Subject,
            Body = mailTemplate.Body,
            IsBodyHtml = mailTemplate.IsBodyHtml,
        };

        mailMessage.To.Add(request.To.ToString());
        mailMessage.CC.Add(request.Cc.ToString());
        mailMessage.Bcc.Add(request.Bcc.ToString());

        await options.Value.SmtpClient.SendMailAsync(mailMessage, cancellationToken);
    }

    private async Task<MailTemplateResult> GetTemplate(string templateCode, object parameters, CancellationToken cancellationToken)
    {
        var template = await templateRepositories.GetByIdAsync(c => c.Code == templateCode, cancellationToken) ?? throw new NullReferenceException($"'{templateCode}' is not found!");
        string body = template.Builder.Build(parameters).ToString();
        return new MailTemplateResult
        {
            Body = body,
            IsBodyHtml = template.IsBodyHtml
        };
    }
}
