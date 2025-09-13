using CodeNet.Email.Models;
using CodeNet.Email.Repositories;
using CodeNet.Email.Settings;
using CodeNet.Messaging.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace CodeNet.Email.Services;

internal class EmailService : IEmailService
{
    private readonly MailOptions _options;
    private readonly MailTemplateRepositories? _templateRepositories;
    private readonly OutboxRepositories? _outboxRepositories;

    public EmailService(IServiceProvider serviceProvider, IOptions<MailOptions> options)
    {
        _options = options.Value;
        if (_options.HasMongoDB)
        {
            _templateRepositories = serviceProvider.GetService<MailTemplateRepositories>();
            _outboxRepositories = serviceProvider.GetService<OutboxRepositories>();
        }
    }

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

    private Task SendMail(MailTemplateResult mailTemplate, SendMailModel request, string templateCode, CancellationToken cancellationToken)
    {
        MailMessage mailMessage = new()
        {
            From = string.IsNullOrEmpty(mailTemplate.From)
                        ? new MailAddress(_options.EmailAddress)
                        : new MailAddress(mailTemplate.From),
            Subject = request.Subject,
            Body = mailTemplate.Body,
            IsBodyHtml = mailTemplate.IsBodyHtml,
        };

        if (!string.IsNullOrWhiteSpace(mailTemplate.To))
            request.To.Add(mailTemplate.To);
        if (!string.IsNullOrWhiteSpace(mailTemplate.Cc))
            request.Cc.Add(mailTemplate.Cc);
        if (!string.IsNullOrWhiteSpace(mailTemplate.Bcc))
            request.Bcc.Add(mailTemplate.Bcc);

        mailMessage.To.Add(request.To.ToString());
        mailMessage.CC.Add(request.Cc.ToString());
        mailMessage.Bcc.Add(request.Bcc.ToString());

        return SendMail(mailMessage, templateCode, cancellationToken);
    }

    public Task SendMail(MailMessage mailMessage, CancellationToken cancellationToken)
    {
        return SendMail(mailMessage, string.Empty, cancellationToken);
    }

    public Task SendMail(MailMessage mailMessage, object? param, CancellationToken cancellationToken)
    {
        var builder = TemplateBuilder.Compile(mailMessage.Body);
        mailMessage.Body = builder.Build(param).ToString();
        return SendMail(mailMessage, cancellationToken);
    }

    private async Task SendMail(MailMessage mailMessage, string templateCode, CancellationToken cancellationToken)
    {
        await _options.SmtpClient.SendMailAsync(mailMessage, cancellationToken);
        if (_options.HasMongoDB)
            await _outboxRepositories!.CreateAsync(new Outbox()
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
        return !_options.HasMongoDB
            ? throw new NotImplementedException("MongoDB is not implemented!")
            : await _templateRepositories!.GetByIdAsync(c => c.Code == templateCode, cancellationToken) ?? throw new NullReferenceException($"'{templateCode}' is not found!");
    }

    private static MailTemplateResult GenerateMailBody(MailTemplate template, object? parameters)
    {
        string body = template.Builder?.Build(parameters).ToString() ?? string.Empty;
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
