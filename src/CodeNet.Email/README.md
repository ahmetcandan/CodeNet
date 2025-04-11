## CodeNet.Email

CodeNet.Email is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Email/) to install CodeNet.Email

```bash
dotnet add package CodeNet.Email
```

### Usage
appSettings.json
```json
{
  "Email": {
    "EmailAddress": "info@code.net",
    "SmtpClient": {...}
  },
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "Email"
  }
}
```
program.cs
```csharp
using CodeNet.Email.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddEmailService(builder.Configuration.GetSection("Email"), 
    c => 
    {
        c.AddMongoDB(builder.Configuration.GetSection("MongoDB"))
    });

var app = builder.Build();
//...
app.Run();
```

Example

```csharp
public class EmailSenderService(IEmailService EmailService)
{
	public Task SendEmailAsync(SendMailRequest request, CancellationToken cancellationToken)
	{
		return EmailService.SendEmailAsync(request, cancellationToken);
	}
}

```

Data
```json
{
    "name": "Ahmet Candan",
    "list": [
        {
            "Date": 2024-10-29T19:23:43.511Z,
            "Amount": 12.34
        },
        {
            "Date": 2024-10-30T19:23:43.511Z,
            "Amount": 13.56
        }
    ]
}
```

Template
```html
Merhaba @name,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    $each(@i in @list){{
    <tr>
        <td>$DateFormat(@i.Date, 'dd/MM/yyyy')</td><td>$NumberFormat(@i.Amount, 'N')</td>
    </tr>}}
</table>

Send date: $Now('dd.MM.yyyy HH:mm')
```
Output
```html
Merhaba Ahmet Candan,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>

    <tr>
        <td>29/10/2024</td><td>12.34</td>
    </tr>
    <tr>
        <td>30/10/2024</td><td>13.56</td>
    </tr>
</table>

Send date: 29.10.2024 19:23
```