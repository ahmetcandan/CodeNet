## CodeNet.Core

CodeNet.BackgroundJob is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.BackgroundJob/) to install CodeNet.BackgroundJob

```bash
dotnet add package CodeNet.BackgroundJob
```

### Usage
appSettings.json
```json
{
  "Redis": {
    "Hostname": "localhost",
    "Port": 6379
  }
}
```

program.cs
```csharp
using CodeNet.BackgroundJob.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBackgroundService<TestService1>("*/17 * * * * ? *")
    .AddBackgroundJobRedis(builder.Configuration.GetSection("Redis"));
//...

var app = builder.Build();
app.UseBackgroundService();
//...
app.Run();
```
