using CodeNet.Email.Models;

namespace CodeNet.Email.Services;

public interface IEmailService
{
    Task SendMail(SendMailRequest request, CancellationToken cancellationToken);
}