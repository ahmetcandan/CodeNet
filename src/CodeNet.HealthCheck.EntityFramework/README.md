## CodeNet.HealthCheck.EntityFramework

CodeNet.HealthCheck.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.EntityFramework/) to install CodeNet.HealthCheck.EntityFramework

```bash
dotnet add package CodeNet.HealthCheck.EntityFramework
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks(options =>
    {
        options.AddEntityFrameworkHealthCheck<XDbContext>();
    });
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```