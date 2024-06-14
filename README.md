![Logo](https://raw.githubusercontent.com/ahmetcandan/ImageHandler/master/ico.png) 
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
```


## CodeNet.EntityFramework

CodeNet.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework/) to install CodeNet.EntityFramework.

```bash
dotnet add package CodeNet.EntityFramework
```

### Usage
#### appSettings.json
```json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=localhost;Initial Catalog=TestDB;TrustServerCertificate=true"
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddSqlServer("SqlServer");
//...

var app = builder.Build();
//...
app.Run();
```


## CodeNet.ExceptionHandling

CodeNet.ExceptionHandling is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.ExceptionHandling/) to install CodeNet.ExceptionHandling.

```bash
dotnet add package CodeNet.ExceptionHandling
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
//...

var app = builder.Build();
//...
app.Run();
```


## CodeNet.Mapper

CodeNet.Mapper is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Mapper/) to install CodeNet.Mapper.

```bash
dotnet add package CodeNet.Mapper
```

#### program.cs
```csharp
using CodeNet.Mapper.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MapperModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```


## CodeNet.MongoDB

CodeNet.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MongoDB/) to install CodeNet.MongoDB.

```bash
dotnet add package CodeNet.MongoDB
```

### Usage
#### appSettings.json
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CodeNet"
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMongoDB("MongoDB");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MongoDBModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```


## CodeNet.RabbitMQ

CodeNet.RabbitMQ is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.RabbitMQ/) to install CodeNet.RabbitMQ.

```bash
dotnet add package CodeNet.RabbitMQ
```

### Usage
#### appSettings.json
```json
{
  "RabbitMQ": {
    "Hostname": "localhost",
    "Username": "guest",
    "Password": "guest",
    "Exchange": "",
    "RoutingKey": "RoutingKey",
    "Queue": "QueueName",
    "Durable": false,
    "AutoDelete": false,
    "Exclusive": false,
    "AutoAck": true
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddRabbitMQ("RabbitMQ");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    //for Producer
    containerBuilder.RegisterModule<RabbitMQProducerModule<QueueModel>>();

    //for Consumer
    containerBuilder.RegisterModule<RabbitMQConsumerModule<QueueModel>>();
});
//...

var app = builder.Build();
//...
app.Run();
```


## CodeNet.Redis

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis.

```bash
dotnet add package CodeNet.Redis
```

### Usage
#### appSettings.json
```json
{
  "Redis": {
    "Hostname": "localhost",
    "Port": 6379
  }
}
```
#### program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
//for Distributed Cache
builder.AddRedisDistributedCache("Redis");

//for Distributed Lock
builder.AddRedisDistributedLock("Redis");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    //for Distributed Cache
    containerBuilder.RegisterModule<RedisDistributedCacheModule>();

    //for Distributed Lock
    containerBuilder.RegisterModule<RedisDistributedLockModule>();
});
//...

var app = builder.Build();
//...
app.Run();
```
