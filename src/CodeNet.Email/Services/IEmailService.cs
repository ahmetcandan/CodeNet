using CodeNet.Email.Models;
using System.Net.Mail;

namespace CodeNet.Email.Services;

public interface IEmailService
{
    Task SendMail(SendMailRequest request, CancellationToken cancellationToken);
    Task SendBulkMail(SendBulkMailRequest request, CancellationToken cancellationToken);
    Task SendMail(MailMessage mailMessage, CancellationToken cancellationToken);
}