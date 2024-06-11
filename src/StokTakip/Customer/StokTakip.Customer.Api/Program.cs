using Autofac.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;
using NetCore.Extensions;
using NetCore.Abstraction.Extensions;
using StokTakip.Customer.Container;
using StokTakip.Customer.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));
builder.AddNetCore("Application");
builder.AddAuthentication("JWT", "public_key.pem");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddMongoDB<MongoDBSettings>("MongoDB");
builder.AddSqlServer<CustomerDbContext>("SqlServer");
builder.AddLogging();

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseNetCore(builder.Configuration, "Application");
app.Run();
