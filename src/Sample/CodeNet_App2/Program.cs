using CodeNet.Core.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet_App2.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));
builder.Services.AddRabbitMQConsumer<ConsumerHandler>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();
app.UseCodeNet();
app.UseRabbitMQConsumer();
app.Run();
