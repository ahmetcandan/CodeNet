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
    .AddStackExcahangePublisher(builder.Configuration.GetSection("StackExchangeRedis"))
    .AddStackExcahangeSubscribe(builder.Configuration.GetSection("StackExchangeRedis"));
builder.Services.AddScoped<IStackExchangeSubscribeHandler<StackExchangeSubscribeService>, MessageSubscribeHandler>();
//...

var app = builder.Build();
app.UseStackExcahangeSubscribe<StackExchangeSubscribeService>();
//...
app.Run();
```
#### Usage Publisher
```csharp
public class MessagePublisher(StackExchangePublisherService Publisher)
{
    public async Task<ResponseBase> Send(MessagePublisherRequest request, CancellationToken cancellationToken)
    {
        await Publisher.Publish(request.Data);
        return new ResponseBase("200", "Successfull");
    }
}
```
#### Usage Subscriber
```csharp
public class MessageSubscribeHandler : IStackExchangeSubscribeHandler<StackExchangeSubscribeService>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```