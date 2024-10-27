using CodeNet.Email.Extensions;
using CodeNet.Email.Services;
using CodeNet.Email.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace CodeNet.Email.Tests;

public class EmailServiceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Send_Email_Test()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddEmailService(new SmtpOptions
        {
            EmailAddress = "candanahm@gmail.com",
            SmtpClient = new()
        });
        var serviceProvider = services.BuildServiceProvider();
        var emailService = serviceProvider.GetRequiredService<IEmailService>();
        string body = @"Merhaba @name,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    $each(@i in @list){{
    <tr>
        <td>$DateFormat(@i.Date, 'dd/MM/yyyy')</td><td>$NumberFormat(@i.Amount, 'N')</td>
    </tr>}}
</table>

Send date: $Now('dd.MM.yyyy HH:mm')";
        object param = new
        {
            name = "Ahmet",
            list = new List<object>
            {
                new
                {
                    Date = DateTime.Now,
                    Amount = 12.456
                },
                new
                {
                    Date = DateTime.Now.AddDays(1),
                    Amount = 15.905
                },
                new
                {
                    Date = DateTime.Now.AddDays(2),
                    Amount = 13
                }
            }
        };
        await emailService.SendMail(new MailMessage
        {
            Body = body
        }, param, CancellationToken.None);

        Assert.That(true);
    }
}