## CodeNet.ApiHost

CodeNet.ApiHost is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.ApiHost/) to install CodeNet.ApiHost

```bash
dotnet add package CodeNet.ApiHost
```

### Usage
program.cs
```csharp
using CodeNet.ApiHost.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBackgroundJob(options =>
    {
        options.AddRedis(builder.Configuration.GetSection("Redis"));
        options.AddScheduleJob<TestService1>("TestService1", TimeSpan.FromSeconds(115), new() { ExpryTime = TimeSpan.FromSeconds(1) });
        options.AddScheduleJob<TestService2>("TestService2", "0 */5 * * *", new() { ExpryTime = TimeSpan.FromSeconds(1) });
        options.AddDbContext(c => c.UseSqlServer(builder.Configuration.GetConnectionString("BackgroundService")!));
        options.AddBasicAuth(new Dictionary<string, string> { { "admin", "Admin123!" } });
    });
//...

var app = builder.Build();
app.UseBackgroundService();
//...
app.Run();
```

