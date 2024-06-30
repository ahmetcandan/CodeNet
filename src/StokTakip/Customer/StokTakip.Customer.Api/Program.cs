using Autofac.Extensions.DependencyInjection;
using StokTakip.Customer.Container;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Contract.Model;
using CodeNet.Core.Extensions;
using CodeNet.Redis.Extensions;
using CodeNet.RabbitMQ.Extensions;
using CodeNet.Elasticsearch.Extensions;
using CodeNet.MongoDB.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.HealthCheck.Extensions;
using CodeNet.HealthCheck.Redis.Extensions;
using CodeNet.HealthCheck.EntityFramework.Extensions;
using CodeNet.HealthCheck.MongoDB.Extensions;
using CodeNet.HealthCheck.RabbitMQ.Extensions;
using CodeNet.HealthCheck.Elasticsearch.Extensions;
using CodeNet.Logging.Extensions;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCodeNetContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));
builder.AddCodeNet("Application");
builder.AddAuthentication("JWT");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.Logging.AddConsole();
//builder.AddRabbitMQConsumer("RabbitMQ");
//builder.AddRabbitMQProducer("RabbitMQ");
builder.AddMongoDB("MongoDB");
builder.AddElasticsearch("Elasticsearch");
builder.AddSqlServer<CustomerDbContext>("SqlServer");
builder.Services.AddHealthChecks()
    .AddCodeNetHealthCheck()
    .AddEntityFrameworkHealthCheck<CustomerDbContext>()
    .AddRedisHealthCheck()
    .AddMongoDbHealthCheck()
    .AddRabbitMqHealthCheck(builder, "RabbitMQ")
    .AddElasticsearchHealthCheck();
builder.AddLogging("Log");

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseCodeNet(builder.Configuration, "Application");
//app.UseRabbitMQConsumer<MongoModel>();
//app.UseLogging();
app.UseHealthChecks("/health");
app.Run();
