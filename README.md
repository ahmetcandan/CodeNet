![Logo](https://raw.githubusercontent.com/ahmetcandan/ImageHandler/master/ico.png) 

---
# CodeNet

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
builder.AddCodeNet("Application");
builder.AddAuthenticationWithAsymmetricKey("JWT");
//...

var app = builder.Build();
app.UseCodeNet(builder.Configuration, "Application");
//...
app.Run();
```

## CodeNet.Elasticsearch

CodeNet.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Elasticsearch/) to install CodeNet.Elasticsearch

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
using CodeNet.Elasticsearch.Extensions;

var builder = WebApplication.CreateBuilder(args);
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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework/) to install CodeNet.EntityFramework

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
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDbContext<CustomerDbContext>("SqlServer");
//or
builder.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(builder.Configuration, "SqlServer"));
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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.InMemory/) to install CodeNet.EntityFramework.InMemory

```bash
dotnet add package CodeNet.EntityFramework.InMemory
```

### Usage
program.cs
```csharp
using CodeNet.EntityFramework.InMemory.Extensions;

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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.MySQL/) to install CodeNet.EntityFramework.MySQL

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
using CodeNet.EntityFramework.MySQL.Extensions;

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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Oracle/) to install CodeNet.EntityFramework.Oracle

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
using CodeNet.EntityFramework.Oracle.Extensions;

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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.PostgreSQL/) to install CodeNet.EntityFramework.PostgreSQL

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
using CodeNet.EntityFramework.PostgreSQL.Extensions;

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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EntityFramework.Sqlite/) to install CodeNet.EntityFramework.Sqlite

```bash
dotnet add package CodeNet.EntityFramework.Sqlite
```

### Usage
appSettings.json
```json
{
  "ConnectionStrings": {
    "Sqlite": "Data Source=mydb.db;Version=3;"
  }
}
```
program.cs
```csharp
using CodeNet.EntityFramework.Sqlite.Extensions;

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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.ExceptionHandling/) to install CodeNet.ExceptionHandling

```bash
dotnet add package CodeNet.ExceptionHandling
```

### Usage
appSettings.json
```json
{
  "DefaultErrorMessage": {
	"MessageCode": "EX0001",
	"Message": "An unexpected error occurred!"
  }
}
```

program.cs
```csharp
using CodeNet.ExceptionHandling.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddDefaultErrorMessage("DefaultErrorMessage")
//...

var app = builder.Build();
//...
app.UseExceptionHandling(); //This should be used last.
app.Run();
```

Example Error Message
```json
{
  "MessageCode": "EX0001",
  "Message": "An unexpected error occurred!"
}
```

## CodeNet.HealthCheck

CodeNet.HealthCheck is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck/) to install CodeNet.HealthCheck

```bash
dotnet add package CodeNet.HealthCheck
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddCodeNetHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HealthCheck.Elasticsearch

CodeNet.HealthCheck.Elasticsearch is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.Elasticsearch/) to install CodeNet.HealthCheck.Elasticsearch

```bash
dotnet add package CodeNet.HealthCheck.Elasticsearch
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Elasticsearch.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddElasticsearchHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HealthCheck.EntityFramework

CodeNet.HealthCheck.EntityFramework is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.EntityFramework/) to install CodeNet.HealthCheck.EntityFramework

```bash
dotnet add package CodeNet.HealthCheck.EntityFramework
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddEntityFrameworkHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HealthCheck.MongoDB

CodeNet.HealthCheck.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.MongoDB/) to install CodeNet.HealthCheck.MongoDB

```bash
dotnet add package CodeNet.HealthCheck.MongoDB
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.MongoDB.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddMongoDbHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HealthCheck.RabbitMQ

CodeNet.HealthCheck.RabbitMQ is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.RabbitMQ/) to install CodeNet.HealthCheck.RabbitMQ

```bash
dotnet add package CodeNet.HealthCheck.RabbitMQ
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.RabbitMQ.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddRabbitMqHealthCheck(builder, "RabbitMQ");
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HealthCheck.Redis

CodeNet.HealthCheck.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HealthCheck.Redis/) to install CodeNet.HealthCheck.Redis

```bash
dotnet add package CodeNet.HealthCheck.Redis
```

### Usage
program.cs
```csharp
using CodeNet.HealthCheck.Redis.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks()
    .AddRedisHealthCheck();
//...

var app = builder.Build();
app.UseHealthChecks("/health");
//...
app.Run();
```

## CodeNet.HttpClient

CodeNet.HttpClient is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.HttpClient/) to install CodeNet.HttpClient

```bash
dotnet add package CodeNet.HttpClient
```

### Usage
program.cs
```csharp
using CodeNet.HttpClient.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddHttpClient();
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.Identity

This is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Identity/) to install CodeNet.Identity

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
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddNetCore("Application")
       .AddAuthenticationWithAsymmetricKey("Identity")
       .AddIdentityWithAsymmetricKey(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");
       

var app = builder.Build();
//...
app.Run();
```

## CodeNet.Logging

CodeNet.Logging is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Logging/) to install CodeNet.Logging

```bash
dotnet add package CodeNet.Logging
```

### Usage
program.cs
```csharp
using CodeNet.Logging.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogging("Logging");
//...

var app = builder.Build();
app.UseLogging();
//...
app.Run();
```

## CodeNet.MakerChecker

CodeNet.MakerChecker is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MakerChecker/) to install CodeNet.MakerChecker

```bash
dotnet add package CodeNet.MakerChecker
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
builder.AddMakerChecker(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");
//...

var app = builder.Build();
//...
app.Run();
```

Example Model
```csharp
public class TestTable : MakerCheckerEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
```
Repository
```csharp
public class TestTableRepository(MakerCheckerDbContext dbContext, ICodeNetContext identityContext) : MakerCheckerRepository<TestTable>(dbContext, identityContext)
{
}
```

## CodeNet.Mapper

CodeNet.Mapper is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Mapper/) to install CodeNet.Mapper

```bash
dotnet add package CodeNet.Mapper
```

### Usage
program.cs
```csharp
using CodeNet.Mapper.Module;

var builder = WebApplication.CreateBuilder(args);
builder.AddMapper();
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.MongoDB

CodeNet.MongoDB is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.MongoDB/) to install CodeNet.MongoDB

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

## CodeNet.Parameters

CodeNet.Parameters is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Parameters/) to install CodeNet.Parameters

```bash
dotnet add package CodeNet.Parameters
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
builder.AddParameters(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");
//...

var app = builder.Build();
//...
app.Run();
```

## CodeNet.RabbitMQ

CodeNet.RabbitMQ is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.RabbitMQ/) to install CodeNet.RabbitMQ

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
public class MessageProducer(IRabbitMQProducerService<QueueModel> Producer)
{
    public async Task<ResponseBase> Send(MessageProducerRequest request, CancellationToken cancellationToken)
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

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Redis/) to install CodeNet.Redis

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
builder
    .AddRedisDistributedCache("Redis")
    .AddRedisDistributedLock("Redis");
//...

var app = builder.Build();
app.UseDistributedCache()
    .UseDistributedLock();
//...
app.Run();
```
#### Usage Lock
```csharp  
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{customerId}")]
    [Lock]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    //...
}
```
#### Usage Cache
```csharp  
using CodeNet.Core.Models;
using CodeNet.Redis.Attributes;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    [HttpGet("{customerId}")]
    [Cache(10)]
    [ProducesResponseType(200, Type = typeof(CustomerResponse))]
    [ProducesDefaultResponseType(typeof(ResponseMessage))]
    public async Task<IActionResult> GetPersonel(int customerId, CancellationToken cancellationToken)
    {
        return Ok(await customerService.GetCustomer(customerId, cancellationToken));
    }

    //...
}
```

---
## Skils
<div align="left">
  <img src="https://cdn.simpleicons.org/dotnet/512BD4" height="30" alt="dot-net logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-original.svg" height="30" alt="csharp logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/microsoftsqlserver/microsoftsqlserver-plain.svg" height="30" alt="microsoftsqlserver logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/oracle/oracle-original.svg" height="30" alt="oracle logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/mysql/mysql-original.svg" height="30" alt="mysql logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postgresql/postgresql-original.svg" height="30" alt="postgresql logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/sqlite/sqlite-original.svg" height="30" alt="sqlite logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/mongodb/mongodb-original.svg" height="30" alt="mongodb logo"  />
  <img width="12" />
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/redis/redis-original.svg" height="30" alt="redis logo"  />
  <img width="12" />
  <img src="https://cdn.simpleicons.org/rabbitmq/FF6600" height="30" alt="rabbitmq logo"  />
</div>

---
## Contact
<div align="left">
    <a href="mailto:candanahm@gmail.com">
  <img src="https://img.shields.io/static/v1?message=Gmail&logo=gmail&label=&color=D14836&logoColor=white&labelColor=&style=for-the-badge" height="35" alt="gmail logo" /></a>
  <a href="https://linkedin.com/in/ahmetcandan">
  <img src="https://img.shields.io/static/v1?message=LinkedIn&logo=linkedin&label=&color=0077B5&logoColor=white&labelColor=&style=for-the-badge" height="35" alt="linkedin logo" /></a>
  <a href="https://x.com/ahmetcndan">
  <img src="https://img.shields.io/static/v1?message=Twitter&logo=twitter&label=&color=1DA1F2&logoColor=white&labelColor=&style=for-the-badge" height="35" alt="twitter logo" /></a>
  <a href="https://stackoverflow.com/users/7229379/ahmet-candan">
  <img src="https://img.shields.io/static/v1?message=Stackoverflow&logo=stackoverflow&label=&color=FE7A16&logoColor=white&labelColor=&style=for-the-badge" height="35" alt="stackoverflow logo"  /></a>
  <a href="https://www.hackerrank.com/profile/ahmetcandan">
  <img src="https://img.shields.io/static/v1?message=HackerRank&logo=hackerrank&label=&color=2EC866&logoColor=white&labelColor=&style=for-the-badge" height="35" alt="hackerrank logo"  /></a>
</div>

---
