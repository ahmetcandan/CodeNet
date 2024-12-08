using CodeNet.Core.Extensions;
using CodeNet.EventBus.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet_App2.Services;

await Task.Delay(2000);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));
//builder.Services.AddRabbitMQConsumer<RabbitMQConsumerHandler>(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.AddKafkaConsumer<KafkaConsumerHandler>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddEventBusSubscriber<EventBusSubscriberHandler>(builder.Configuration.GetSection("EventBus"));

var app = builder.Build();
app.UseCodeNet(c => c.UseSwagger());
//app.UseRabbitMQConsumer();
app.UseEventBusSubscriber();
//app.UseKafkaConsumer();
app.Run();
