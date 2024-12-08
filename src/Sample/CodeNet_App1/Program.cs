using CodeNet.Core.Extensions;
using CodeNet.EventBus.Extensions;
using CodeNet.EventBus.Server;
using CodeNet.HealthCheck.Extensions;
using CodeNet.HealthCheck.MongoDB.Extensions;
using CodeNet.HealthCheck.RabbitMQ.Extensions;
using CodeNet.HealthCheck.Redis.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet.RabbitMQ.Outbox.Extensions;
using CodeNet_App1.Services;
using CodeNet_App1.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));

CodeNetEventBus eventBus = new(7777);
eventBus.Start();

Console.ReadLine();

//builder.Services.AddRabbitMQOutboxModule(builder.Configuration.GetSection("Outbox"), c =>
//{
//    c.AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"));
//    c.AddMongoDB(builder.Configuration.GetSection("OutboxMongoDB"));
//    c.AddRedis(builder.Configuration.GetSection("Redis"));
//});
//builder.Services.AddHealthChecks(c =>
//{
//    c.AddRabbitMqHealthCheck(c.Services, builder.Configuration.GetSection("RabbitMQ"), "RabbitMQ");
//    c.AddMongoDbHealthCheck(builder.Configuration.GetSection("OutboxMongoDB"), "OutboxMongoDB");
//    c.AddRedisHealthCheck(builder.Configuration.GetSection("Redis"), "Redis");
//});
builder.Services.AddEventBusPublisher(builder.Configuration.GetSection("EventBus"));
//builder.Services.AddKafkaProducer(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<ProducerOptions>(builder.Configuration.GetSection("ProducerOptions"));


var app = builder.Build();
app.UseCodeNet(c => c.UseSwagger());
var options = app.Services.GetRequiredService<IOptions<ProducerOptions>>();
for (int i = 0; i < options.Value.TotalSeconds; i++)
{
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {i + 1:000}. Second Start...");
    await Task.Run(async () =>
    {
        FireAndForget.Execute(app.Services, options.Value.MessagePerSecond, i + 1);
    });
    await Task.Delay(TimeSpan.FromSeconds(1));
}
//app.UseRabbitMQOutboxModule();
//app.UseCodeNetHealthChecks();
app.Run();
