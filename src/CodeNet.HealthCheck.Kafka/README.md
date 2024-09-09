## CodeNet.HealthCheck.Kafka

CodeNet.HealthCheck.Kafka is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.Kafka/) to install CodeNet.HealthCheck.Kafka

```bash
dotnet add package CodeNet.HealthCheck.Kafka
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Kafka.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddKafkaHealthCheck(builder.Services, builder.Configuration.GetSection("Kafka"));
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```