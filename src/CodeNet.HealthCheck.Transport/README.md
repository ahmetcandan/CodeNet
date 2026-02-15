## CodeNet.HealthCheck.EventBus

CodeNet.HealthCheck.EventBus is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.EventBus/) to install CodeNet.HealthCheck.EventBus

```bash
dotnet add package CodeNet.HealthCheck.EventBus
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.EventBus.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks(options =>
    {
        options.AddEventBusHealthCheck();
    });
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```