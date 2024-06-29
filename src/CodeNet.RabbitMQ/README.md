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