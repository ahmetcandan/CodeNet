## CodeNet.HealthCheck.MongoDB

CodeNet.HealthCheck.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.MongoDB/) to install CodeNet.HealthCheck.MongoDB

```bash
dotnet add package CodeNet.HealthCheck.MongoDB
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.MongoDB.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddMongoDbHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```