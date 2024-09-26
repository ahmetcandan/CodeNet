using CodeNet.Core.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet_App1.Services;
using CodeNet_App1.Settings;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));
builder.Services.AddRabbitMQProducer(builder.Configuration.GetSection("RabbitMQ"));
//builder.Services.AddKafkaProducer(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<ProducerOptions>(builder.Configuration.GetSection("ProducerOptions"));

var app = builder.Build();
app.UseCodeNet();
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
app.Run();
