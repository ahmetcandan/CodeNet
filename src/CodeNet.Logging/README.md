## CodeNet.Logging

CodeNet.Logging is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Logging/) to install CodeNet.Logging.

```bash
dotnet add package CodeNet.Logging
```

#### program.cs
```csharp
using CodeNet.Logging.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<LoggingModule>();
});
//..

var app = builder.Build();
//..
app.Run();
```