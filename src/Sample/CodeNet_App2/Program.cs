using CodeNet.Core.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet_App2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));
builder.Services.AddRabbitMQConsumer<RabbitMQConsumerHandler>(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.AddKafkaConsumer<KafkaConsumerHandler>(builder.Configuration.GetSection("Kafka"));

var app = builder.Build();
app.UseCodeNet();
app.UseRabbitMQConsumer();
//app.UseKafkaConsumer();
app.Run();
