using CodeNet.Email.Extensions;
using CodeNet.Email.Settings;
using CodeNet.Messaging.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Email.Tests;

public class EmailServiceTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Send_Email_Test()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddEmailService(new SmtpOptions
        {
            EmailAddress = "candanahm@gmail.com",
            SmtpClient = new()
        });
        string body = @"Merhaba @name,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    $each(@item in @list){{
    <tr>
        <td>$DateFormat(@item.Date, 'dd-MM-yyyy')</td><td>$NumberFormat(@item.Amount, 'N')</td>
    </tr>}}
</table>

$if(@name == 'Ahmet'){{
    Name is @name
}}
$else{{
    Name is not Ahmet
}}

Send date: $DateFormat(@date, 'dd.MM.yyyy HH:mm')";
        string expected = @"Merhaba Ahmet,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    
    <tr>
        <td>08-04-2025</td><td>12.46</td>
    </tr>
    <tr>
        <td>09-04-2025</td><td>15.90</td>
    </tr>
    <tr>
        <td>10-04-2025</td><td>13.00</td>
    </tr>
</table>


    Name is Ahmet


Send date: 08.04.2025 22:55";

        DateTime date = new(2025, 4, 8, 22, 55, 0);
        object param = new
        {
            name = "Ahmet",
            date,
            list = new List<object>
            {
                new
                {
                    Date = date,
                    Amount = 12.456
                },
                new
                {
                    Date = date.AddDays(1),
                    Amount = 15.905
                },
                new
                {
                    Date = date.AddDays(2),
                    Amount = 13
                }
            }
        };

        var builder = BodyBuilder.Compile(body);

        var txt = builder.Build(param).ToString() ?? string.Empty;


        Assert.That(txt, Is.EqualTo(expected));
    }
}