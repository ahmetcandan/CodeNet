using Autofac.Extensions.DependencyInjection;
using CodeNet.Extensions;
using StokTakip.Customer.Container;
using StokTakip.Customer.Repository;
using StokTakip.Customer.Contract.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));
builder.AddNetCore("Application");
builder.AddAuthentication("JWT", "public_key.pem");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddRabbitMQ("RabbitMQ");
builder.AddMongoDB("MongoDB");
builder.AddElasticsearch("Elasticsearch");
builder.AddSqlServer<CustomerDbContext>("SqlServer");
builder.AddLogging();

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseNetCore(builder.Configuration, "Application");
app.UseRabbitMQConsumer<KeyValueModel>();
app.Run();
