## CodeNet.HealthCheck.Redis

CodeNet.HealthCheck.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.Redis/) to install CodeNet.HealthCheck.Redis

```bash
dotnet add package CodeNet.HealthCheck.Redis
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Redis.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddRedisHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```