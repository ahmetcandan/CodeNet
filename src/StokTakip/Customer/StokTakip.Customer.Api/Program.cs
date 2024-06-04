using Autofac.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;
using NetCore.Core.Extensions;
using StokTakip.Customer.Container;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));

builder.AddNetCore("Application");
builder.AddAuthentication("JWT", "public_key.pem");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddSqlServer("SqlServer");
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseNetCore(builder.Configuration, "Application");
app.Run();
