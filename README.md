![Logo](https://raw.githubusercontent.com/ahmetcandan/ImageHandler/master/ico.png) 
# CodeNet

## CodeNet.Core

CodeNet.Core is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Core/) to install CodeNet.Core.

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

## CodeNet.Elasticsearch

CodeNet.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Elasticsearch/) to install CodeNet.Elasticsearch.

```bash
dotnet add package CodeNet.Elasticsearch
```

### Usage
appSettings.json
```json
{
  "Elasticsearch": {
    "Hostname": "localhost:9200",
    "Username": "elastic",
    "Password": "password"
  }
}
```
program.cs
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
Repository
```csharp
public class TestElasticRepository : ElasticsearchRepository<ElasticModel>
{
    public TestElasticRepository(ElasticsearchDbContext dbContext) : base(dbContext)
    {
        //...
    }
}
```
Model
```csharp
[IndexName("Test")]
public class ElasticModel : IElasticsearchModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
```

## CodeNet.EntityFramework

CodeNet.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework/) to install CodeNet.EntityFramework.

```bash
dotnet add package CodeNet.EntityFramework
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=localhost;Initial Catalog=TestDB;TrustServerCertificate=true"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
});
builder.AddSqlServer<CustomerDbContext>("SqlServer");
//...

var app = builder.Build();
//...
app.Run();
```
DbContext
```csharp
public partial class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public virtual DbSet<Model.Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
}
```
Repository
```csharp
public class CustomerRepository(CustomerDbContext context, IIdentityContext identityContext) : 
    TracingRepository<Model.Customer>(context, identityContext), ICustomerRepository
{
}
```

Repository Usage
```csharp
public class CustomerService(ICustomerRepository CustomerRepository, IAutoMapperConfiguration Mapper) : BaseService, ICustomerService
{
    public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var model = Mapper.MapObject<CreateCustomerRequest, Model.Customer>(request);
        var result = await CustomerRepository.AddAsync(model, cancellationToken);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> DeleteCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken);
        CustomerRepository.Remove(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse?> GetCustomer(int customerId, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([customerId], cancellationToken) ?? throw new UserLevelException("01", "Kullanıcı bulunamadı!");
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }

    public async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await CustomerRepository.GetAsync([request.Id], cancellationToken);
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.No = request.No;
        CustomerRepository.Update(result);
        await CustomerRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Customer, CustomerResponse>(result);
    }
}
```

## CodeNet.EntityFramework.InMemory

CodeNet.EntityFramework.InMemory is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.InMemory/) to install CodeNet.EntityFramework.

```bash
dotnet add package CodeNet.EntityFramework.InMemory
```

### Usage
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddInMemoryDB("DatabaseName");
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.EntityFramework.MySQL

CodeNet.EntityFramework.MySQL is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.MySQL/) to install CodeNet.EntityFramework.MySQL.

```bash
dotnet add package CodeNet.EntityFramework.MySQL
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "MySQL": "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMySQL("MySQL");
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.EntityFramework.Oracle

CodeNet.EntityFramework.Oracle is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Oracle/) to install CodeNet.EntityFramework.Oracle.

```bash
dotnet add package CodeNet.EntityFramework.Oracle
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=MyOracleDB;Integrated Security=yes;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddOracle("Oracle");
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.EntityFramework.PostgreSQL

CodeNet.EntityFramework.PostgreSQL is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.PostgreSQL/) to install CodeNet.EntityFramework.PostgreSQL.

```bash
dotnet add package CodeNet.EntityFramework.PostgreSQL
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddNpgsql("PostgreSQL");
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.EntityFramework.Sqlite

CodeNet.EntityFramework.Sqlite is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Sqlite/) to install CodeNet.EntityFramework.Sqlite.

```bash
dotnet add package CodeNet.EntityFramework.Sqlite
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "Sqlite": "Data Source=c:\mydb.db;Version=3;"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddSqlite("Sqlite");
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

## CodeNet.Logging

CodeNet.Logging is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Logging/) to install CodeNet.Logging.

```bash
dotnet add package CodeNet.Logging
```

### Usage
program.cs
```csharp
using CodeNet.Logging.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<LoggingModule>();
});
builder.AddLogging("Logging");
//...

var app = builder.Build();
app.UseLogging();
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

### Usage
program.cs
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
appSettings.json
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "CodeNet"
  }
}
```
program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.AddMongoDB("MongoDB");
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.AddModule<MongoDBModule>();
    containerBuilder.RegisterType<SampleRepository>().As<ISampleRepository>().InstancePerLifetimeScope();
});
//...

var app = builder.Build();
//...
app.Run();
```
Sample Repositoriy
```csharp
public class SampleRepository(MongoDBContext dbContext) : BaseMongoRepository<KeyValueModel>(dbContext), ISampleRepository
{
    //...
}
```
KeyValueModel
```csharp
[CollectionName("KeyValue")]
public class KeyValueModel : BaseMongoDBModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
```

## CodeNet.RabbitMQ

CodeNet.RabbitMQ is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.RabbitMQ/) to install CodeNet.RabbitMQ.

```bash
dotnet add package CodeNet.RabbitMQ
```

### Usage
appSettings.json
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
program.cs
```csharp
using CodeNet.Core.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet.RabbitMQ.Module;
using ExampleApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.AddModule<RabbitMQProducerModule<QueueModel>>();
    containerBuilder.AddModule<RabbitMQConsumerModule<QueueModel>>();
    containerBuilder.RegisterType<MessageConsumerHandler>().As<IRabbitMQConsumerHandler<QueueModel>>().InstancePerLifetimeScope();
});
builder
    .AddRabbitMQProducer("RabbitMQ")
    .AddRabbitMQConsumer("RabbitMQ");
//...

var app = builder.Build();
app.UseRabbitMQConsumer<QueueModel>();
//...
app.Run();
```
#### Usage Producer
```csharp
public class MessageProducerHandler(IRabbitMQProducerService<QueueModel> Producer) : IRequestHandler<MessageProducerRequest, ResponseBase>
{
    public async Task<ResponseBase> Handle(MessageProducerRequest request, CancellationToken cancellationToken)
    {
        Producer.Publish(request.Data);
        return new ResponseBase("200", "Successfull");
    }
}
```
#### Usage Consumer
```csharp
public class MessageConsumerHandler : IRabbitMQConsumerHandler<KeyValueModel>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```

## CodeNet.Redis

CodeNet.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis.

```bash
dotnet add package CodeNet.Redis
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
using CodeNet.Container.Extensions;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.Redis.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    //for cache
    containerBuilder.AddModule<RedisDistributedCacheModule>();

    //for lock
    containerBuilder.AddModule<RedisDistributedLockModule>();
});
builder
    .AddRedisDistributedCache("Redis")
    .AddRedisDistributedLock("Redis");
//...

var app = builder.Build();
//...
app.Run();
```
#### Usage Lock
```csharp  
using MediatR;
using RedLockNet;

namespace ExampleApi.Handler;

public class TestRequestHandler(IDistributedLockFactory LockFactory) : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        using var redLock = await LockFactory.CreateLockAsync("LOCK_KEY", TimeSpan.FromSeconds(3));
        if (!redLock.IsAcquired)
            throw new SynchronizationLockException();

        //Process...
    }
}
```
Or
```csharp  
using CodeNet.Redis.Attributes;
using MediatR;

namespace ExampleApi.Handler;

public class TestRequestHandler() : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    [Lock(ExpiryTime = 3)]
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        //Process...
    }
}
```
#### Usage Cache
```csharp  
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ExampleApi.Handler;

public class TestRequestHandler(IDistributedLockFactory LockFactory) : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    private const string CACHE_KEY = "KEY";
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        var cacheJsonValue = await DistributedCache.GetStringAsync(CACHE_KEY, cancellationToken);
        if (string.IsNullOrEmpty(cacheJsonValue))
        {
            //Process...
            var response = ...
            await DistributedCache.SetStringAsync(CACHE_KEY, JsonConvert.SerializeObject(response), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            }, cancellationToken);
            return response;
        }
        return JsonConvert.DeserializeObject<ResponseBase<TestResponse>>(cacheJsonValue);
    }
}
```
Or
```csharp  
using CodeNet.Redis.Attributes;
using MediatR;

namespace ExampleApi.Handler;

public class TestRequestHandler() : IRequestHandler<TestRequest, ResponseBase<TestResponse>>
{
    [Cache(Time = 60)]
    public async Task<ResponseBase<TestResponse>> Handle(TestRequest request, CancellationToken cancellationToken)
    {
        //Process...
    }
}
```

## Contact
> [![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/ahmetcandan)

> candanahm@gmail.com
