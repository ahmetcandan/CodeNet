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
    .AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"))
    .AddRabbitMQConsumer(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddScoped<IRabbitMQConsumerHandler<RabbitMQConsumerService>, MessageConsumerHandler>();
//...

var app = builder.Build();
app.UseRabbitMQConsumer<RabbitMQConsumerService>();
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
#### Usage Consumer
```csharp
public class MessageConsumerHandler : IRabbitMQConsumerHandler<RabbitMQConsumerService>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```