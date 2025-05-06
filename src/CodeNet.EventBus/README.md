## CodeNet.EventBus

CodeNet.EventBus is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.EventBus/) to install CodeNet.EventBus

```bash
dotnet add package CodeNet.EventBus
```

### Usage
appSettings.json
```json
{
  "EventBus": {
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
    .AddEventBusSubscriber(builder.Configuration.GetSection("EventBus"))
    .AddEventBusPublisher(builder.Configuration.GetSection("EventBus"));
builder.Services.AddScoped<ISubscriberHandler<EventBusSubscriberService>, SubscriberHandler>();
//...

var app = builder.Build();
app.UseEventBusSubscriber();
//...
app.Run();
```
#### Usage Producer
```csharp
public class MessagePubliser(EventBusPublisherService Publisher)
{
    public async Task<ResponseBase> Send(MessageProducerRequest request, CancellationToken cancellationToken)
    {
        Publisher.Publish(request.Data);
        return new ResponseBase("200", "Successfull");
    }
}
```
#### Usage Consumer
```csharp
public class SubscriberHandler : ISubscriberHandler<EventBusSubscriberService>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```