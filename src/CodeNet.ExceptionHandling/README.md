## CodeNet.ExceptionHandling

CodeNet.ExceptionHandling is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.ExceptionHandling/) to install CodeNet.ExceptionHandling

```bash
dotnet add package CodeNet.ExceptionHandling
```

### Usage
program.cs
```csharp
using CodeNet.ExceptionHandling.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<ExceptionHandlingModule>();
});
//...

var app = builder.Build();
app.UseErrorController();
//...
app.Run();
```