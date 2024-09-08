## CodeNet.Kafka

CodeNet.Kafka is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Kafka/) to install CodeNet.Kafka

```bash
dotnet add package CodeNet.Kafka
```

### Usage
appSettings.json
```json
{
  "Kafka": {
  }
}
```
program.cs
```csharp
using CodeNet.Core.Extensions;
using CodeNet.Kafka.Extensions;
using ExampleApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddKafkaProducer(builder.Configuration.GetSection("Kafka"))
    .AddKafkaConsumer(builder.Configuration.GetSection("Kafka"));
builder.Services.AddScoped<IKafkaConsumerHandler<KafkaConsumerService>, MessageConsumerHandler>();
//...

var app = builder.Build();
app.UseKafkaConsumer<KafkaConsumerService>();
//...
app.Run();
```
#### Usage Producer
```csharp
public class MessageProducer(KafkaProducerService Producer)
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
public class MessageConsumerHandler : IKafkaConsumerHandler<KafkaConsumerService>
{
    public void Handler(ReceivedMessageEventArgs<KeyValueModel> args)
    {
        Console.WriteLine($"MessageId: {args.MessageId}, Value: {args.Data.Value}");
    }
}
```