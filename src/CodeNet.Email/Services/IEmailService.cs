using CodeNet.Email.Models;
using System.Net.Mail;

namespace CodeNet.Email.Services;

/// <summary>
/// Email Sender Service
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send Mail
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendMail(SendMailRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Send Multi Mail
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendBulkMail(SendBulkMailRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Send Mail
    /// </summary>
    /// <param name="mailMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendMail(MailMessage mailMessage, CancellationToken cancellationToken);

    /// <summary>
    /// Send Mail with Params
    /// </summary>
    /// <param name="mailMessage"></param>
    /// <param name="param"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendMail(MailMessage mailMessage, object? param, CancellationToken cancellationToken);
}