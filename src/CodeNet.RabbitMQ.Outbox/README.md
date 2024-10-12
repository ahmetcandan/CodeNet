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
    "RoutingKey": "TQ0210",
    "Queue": "TQ0210",
    "Durable": false,
    "AutoDelete": false,
    "Exclusive": false,
    "DeclareQueue": false,
    "QueueBind": false,
    "ConnectionFactory": {
      "Hostname": "192.168.1.100",
      "Port": 5672
    }
  },
  "Outbox": {
    "SendPeriod": "00:00:01.000",
    "PrefetchCount": 1000,
    "LockSettings": {
      "LockTime": "00:01:00.000",
      "TimeOut": "00:01:00.000"
    }
  },
  "OutboxMongoDB": {
    "ConnectionString": "mongodb://192.168.1.100:27017",
    "DatabaseName": "NetCore"
  },
  "Redis": {
    "Hostname": "192.168.1.100",
    "Port": 6379
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
builder.Services.AddRabbitMQOutboxModule(builder.Configuration.GetSection("Outbox"), c =>
{
    c.AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"));
    c.AddMongoDB(builder.Configuration.GetSection("OutboxMongoDB"));
    c.AddRedis(builder.Configuration.GetSection("Redis"));
});
//...

var app = builder.Build();
app.UseRabbitMQOutboxModule();
//...
app.Run();
```
#### Usage Producer
```csharp
public class MessageProducer(RabbitMQProducerService Producer)
{
    public async Task<ResponseBase> Send(MessageProducerRequest request, CancellationToken cancellationToken)
    {
        Producer.Publish(request.Data);
        return new ResponseBase("200", "Successfull");
    }
}
```
