using Autofac.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;
using NetCore.Core.Extension;
using StokTakip.Customer.Container;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));

var applicationSettings = builder.Configuration.GetSection("Application").Get<ApplicationSettings>()!;
builder.Services.AddNetCore(applicationSettings);
builder.Services.AddAuthentication(builder.Configuration["JWT:ValidAudience"]!, builder.Configuration["JWT:ValidIssuer"]!, "public_key.pem");
builder.Services.AddRedisSettings(builder.Configuration["Redis:Hostname"]!, int.Parse(builder.Configuration["Redis:Port"]!));
builder.Services.AddSqlServer(builder.Configuration.GetConnectionString("SqlServer")!);
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

app.UseNetCore(applicationSettings);
app.Run();
