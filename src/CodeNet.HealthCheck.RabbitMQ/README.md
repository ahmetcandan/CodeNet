## CodeNet.HealthCheck.RabbitMQ

CodeNet.HealthCheck.RabbitMQ is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.RabbitMQ/) to install CodeNet.HealthCheck.RabbitMQ

```bash
dotnet add package CodeNet.HealthCheck.RabbitMQ
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddRabbitMqHealthCheck(builder.Services, builder.Configuration.GetSection("RabbitMQ"));
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```