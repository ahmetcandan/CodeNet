![Logo](https://raw.githubusercontent.com/ahmetcandan/CodeNet/master/ico.png?token=GHSAT0AAAAAACTEDK6REAO552UCPON4H7LCZTMCHLA) 
# CodeNet

## CodeNet.Abstraction

CodeNet.Abstraction is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Abstraction/) to install CodeNet.Abstraction.

```bash
dotnet add package CodeNet.Abstraction
```


## CodeNet.Cache

CodeNet.Cache is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Cache/) to install CodeNet.Cache.

```bash
dotnet add package CodeNet.Cache
```


## CodeNet.Container

CodeNet.Container is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Container/) to install CodeNet.Container.

```bash
dotnet add package CodeNet.Container
```


## CodeNet.Core

CodeNet.Core is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Core/) to install CodeNet.Core.

```bash
dotnet add package CodeNet.Core
```


## CodeNet.Elasticsearch

CodeNet.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Elasticsearch/) to install CodeNet.Elasticsearch.

```bash
dotnet add package CodeNet.Elasticsearch
```

### Usage
#### appSettings.json
```json
{
  "Elasticsearch": {
    "Username": "elastic",
    "Password": "password",
    "Hostname": "localhost"
  }
}
```
#### program.cs
```csharp
using CodeNet.Elasticsearch.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<ElasticsearchModule>();
});
builder.AddElasticsearch("Elasticsearch");
//...

var app = builder.Build();
//...
app.Run();
var app = builder.Build();
//...
app.Run();
```

## CodeNet.Identity

CodeNet.Identity is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Identity/) to install CodeNet.Identity.

```bash
dotnet add package CodeNet.Identity
```

### Usage
#### appSettings.json
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
#### program.cs
```csharp
using Autofac;
using CodeNet.Container.Module;
using CodeNet.Extensions;
using CodeNet.Identity.Api.Handler;
using CodeNet.Identity.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterModule<IdentityModule>();
});
builder.AddNetCore("Application");
builder.AddAuthentication("Identity");
builder.AddIdentity("SqlServer", "Identity");

var app = builder.Build();

app.UseNetCore(builder.Configuration, "Application");
app.Run();
```
