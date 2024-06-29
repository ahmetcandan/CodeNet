## CodeNet.Core

CodeNet.Core is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Core/) to install CodeNet.Core

```bash
dotnet add package CodeNet.Core
```

### Usage
appSettings.json
```json
{
  "Application": {
    "Name": "Customer",
    "Title": "StokTakip | Customer API",
    "Version": "v1.0"
  },
  "JWT": {
    "ValidAudience": "http://codenet",
    "ValidIssuer": "http://login.codenet",
    "PublicKeyPath": "public_key.pem"
  }
}
```
program.cs
```csharp
using CodeNet.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<CodeNetModule>();
    containerBuilder.RegisterModule<MediatRModule>();
});
builder.AddCodeNet("Application");
builder.AddAuthentication("JWT");
//...

var app = builder.Build();
app.UseCodeNet(builder.Configuration, "Application");
//...
app.Run();
```
