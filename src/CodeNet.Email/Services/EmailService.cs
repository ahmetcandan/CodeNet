using System.Net.Mail;
using CodeNet.Email.Models;
using Microsoft.Extensions.Options;
using CodeNet.Email.Settings;
using CodeNet.Email.Repositories;

namespace CodeNet.Email.Services;

internal class EmailService(MailTemplateRepositories templateRepositories, OutboxRepositories outboxRepositories, IOptions<MailOptions> options) : IEmailService
{
    public async Task SendMail(SendMailRequest request, CancellationToken cancellationToken)
    {
        var template = await GetMailTemplate(request.TemplateCode, cancellationToken);
        var mailBody = GenerateMailBody(template, request.Params);
        await SendMail(mailBody, request, request.TemplateCode, cancellationToken);
    }

    public async Task SendBulkMail(SendBulkMailRequest request, CancellationToken cancellationToken)
    {
        var template = await GetMailTemplate(request.TemplateCode, cancellationToken);
        foreach (var item in request.MailInfos)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var mailBody = GenerateMailBody(template, item.Param);
            await SendMail(mailBody, new()
            {
                To = item.To,
                Cc = item.Cc,
                Bcc = item.Bcc,
                Params = item.Param,
                Subject = request.Subject,
            }, request.TemplateCode, cancellationToken);
        }
    }

    private async Task SendMail(MailTemplateResult mailTemplate, SendMailModel request, string templateCode, CancellationToken cancellationToken)
    {
        MailMessage mailMessage = new()
        {
            From = string.IsNullOrEmpty(mailTemplate.From)
                        ? new MailAddress(options.Value.EmailAddress)
                        : new MailAddress(mailTemplate.From),
            Subject = request.Subject,
            Body = mailTemplate.Body,
            IsBodyHtml = mailTemplate.IsBodyHtml,
        };

        request.To.Add(mailTemplate.To);
        request.Cc.Add(mailTemplate.Cc);
        request.Bcc.Add(mailTemplate.Bcc);

        mailMessage.To.Add(request.To.ToString());
        mailMessage.CC.Add(request.Cc.ToString());
        mailMessage.Bcc.Add(request.Bcc.ToString());

        await SendMail(mailMessage, templateCode, cancellationToken);
    }

    public Task SendMail(MailMessage mailMessage, CancellationToken cancellationToken)
    {
        return SendMail(mailMessage, string.Empty, cancellationToken);
    }

    private async Task SendMail(MailMessage mailMessage, string templateCode, CancellationToken cancellationToken)
    {
        await options.Value.SmtpClient.SendMailAsync(mailMessage, cancellationToken);
        if (options.Value.HasMongoDB)
            await outboxRepositories.CreateAsync(new Outbox()
            {
                Id = Guid.NewGuid(),
                Bcc = mailMessage.Bcc,
                Body = mailMessage.Body,
                Cc = mailMessage.CC,
                From = mailMessage.From?.Address ?? string.Empty,
                SendDate = DateTime.UtcNow,
                TemplateCode = templateCode,
                To = mailMessage.To
            }, cancellationToken);
    }

    private async Task<MailTemplate> GetMailTemplate(string templateCode, CancellationToken cancellationToken)
    {
        if (!options.Value.HasMongoDB)
            throw new NotImplementedException("MongoDB is not implemented!");

        return await templateRepositories.GetByIdAsync(c => c.Code == templateCode, cancellationToken) ?? throw new NullReferenceException($"'{templateCode}' is not found!");
    }

    private static MailTemplateResult GenerateMailBody(MailTemplate template, object parameters)
    {
        string body = template.Builder.Build(parameters).ToString();
        return new MailTemplateResult
        {
            Body = body,
            IsBodyHtml = template.IsBodyHtml,
            From = template.From,
            To = template.To,
            Cc = template.Cc,
            Bcc = template.Bcc,
        };
    }
}
