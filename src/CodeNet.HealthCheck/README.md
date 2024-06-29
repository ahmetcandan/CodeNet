## CodeNet.HealthCheck

CodeNet.HealthCheck is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck/) to install CodeNet.HealthCheck

```bash
dotnet add package CodeNet.HealthCheck
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCodeNetHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```