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