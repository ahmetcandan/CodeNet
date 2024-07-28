## CodeNet.StackExchange.Redis

CodeNet.StackExchange.Redis is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.StackExchange.Redis/) to install CodeNet.StackExchange.Redis

```bash
dotnet add package CodeNet.StackExchange.Redis
```

### Usage
appSettings.json
```json
{
  "StackExchangeRedis": {
    "Configuration": "localhost:6379",
    "Channel": "test-channel"
  }
}
```
program.cs
```csharp
using CodeNet.Core.Extensions;
using CodeNet.StackExchange.Redis.Extensions;
using CodeNet.StackExchange.Redis.Module;
using ExampleApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddStackExcahangeProducer(builder.Configuration.GetSection("StackExchangeRedis"))
    .AddStackExcahangeConsumer(builder.Configuration.GetSection("StackExchangeRedis"));
builder.Services.AddScoped<IStackExchangeConsumerHandler<StackExchangeConsumerService>, MessageConsumerHandler>();
//...

var app = builder.Build();
app.UseStackExcahangeConsumer<StackExchangeConsumerService>();
//...
app.Run();
```
#### Usage Producer
```csharp
public class MessageProducer(StackExchangeProducerService Producer)
{
    public async Task<ResponseBase> Send(MessageProducerRequest request, CancellationToken cancellationToken)
    {
        await Producer.Publish(request.Data);
        return new ResponseBase("200", "Successfull");
    }
}
```
#### Usage Consumer
```csharp
public class MessageConsumerHandler : IStackExchangeConsumerHandler<StackExchangeConsumerService>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```