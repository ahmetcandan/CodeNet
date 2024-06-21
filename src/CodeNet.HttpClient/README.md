## CodeNet.HttpClient

CodeNet.HttpClient is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HttpClient/) to install CodeNet.HttpClient.

```bash
dotnet add package CodeNet.HttpClient
```

### Usage
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<HttpClientModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```