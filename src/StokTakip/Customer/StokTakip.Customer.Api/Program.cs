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
using CodeNet.Logging.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCodeNetContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));
builder.AddCodeNet("Application");
builder.AddAuthentication("JWT");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddRabbitMQConsumer("RabbitMQ");
builder.AddRabbitMQProducer("RabbitMQ");
builder.AddMongoDB("MongoDB");
builder.AddElasticsearch("Elasticsearch");
builder.AddSqlServer<CustomerDbContext>("SqlServer");
builder.AddLogging("Logging");

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseCodeNet(builder.Configuration, "Application");
app.UseRabbitMQConsumer<KeyValueModel>();
app.UseLogging();
app.Run();
