using Autofac.Extensions.DependencyInjection;
using NetCore.Extensions;
using StokTakip.Campaign.Container;
using StokTakip.Campaign.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));
builder.AddNetCore("Application");
builder.AddAuthentication("JWT", "public_key.pem");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddSqlServer<CampaignDbContext>("SqlServer");
builder.AddLogging();

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseNetCore(builder.Configuration, "Application");
app.Run();
