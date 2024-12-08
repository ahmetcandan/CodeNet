using CodeNet.Core.Extensions;
using CodeNet.EventBus.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet_App3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"));
//builder.Services.AddRabbitMQConsumer<ConsumerHandler>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddEventBusSubscriber<EventBusSubscriberHandler>(builder.Configuration.GetSection("EventBus"));

var app = builder.Build();
app.UseCodeNet();
//app.UseRabbitMQConsumer();
app.UseEventBusSubscriber();
app.Run();
