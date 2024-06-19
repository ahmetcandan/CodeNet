## CodeNet.Identity

This is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Identity/) to install CodeNet.Identity.

```bash
dotnet add package CodeNet.Identity
```

### Usage
appSettings.json
```json
{
  "Application": {
    "Name": "Identity",
    "Title": "CodeNet | Identity API",
    "Version": "v1.0"
  },
  "ConnectionStrings": {
    "SqlServer": "Data Source=localhost;Initial Catalog=CodeNet;TrustServerCertificate=true"
  },
  "Identity": {
    "ValidAudience": "http://code.net",
    "ValidIssuer": "http://login.code.net",
    "ExpiryTime": 5.0,
    "PublicKeyPath": "public_key.pem",
    "PrivateKeyPath": "private_key.pem"
  }
}
```
program.cs
```csharp
using Autofac;
using CodeNet.Container.Module;
using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.Identity.Api.Handler;
using CodeNet.Identity.Module;
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<CodeNetModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterModule<IdentityModule>();
});

builder.AddNetCore("Application")
       .AddAuthentication("Identity")
       .AddIdentity(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");

builder.Build()
    .UseNetCore(builder.Configuration, "Application")
    .Run();
```
