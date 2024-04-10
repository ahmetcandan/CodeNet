using Autofac;
using Autofac.Extensions.DependencyInjection;
using NetCore.Abstraction.Model;
using NetCore.Core.Extension;
using StokTakip.Customer.Container;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => Bootstrapper.RegisterModules(containerBuilder));

const string appName = "StokTakip | Customer API";
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(appName);
builder.Services.AddAuthentication(builder.Configuration["JWT:ValidAudience"]!, builder.Configuration["JWT:ValidIssuer"]!, "public_key.pem");
builder.Services.AddRedisSettings(builder.Configuration["Redis:Hostname"]!, int.Parse(builder.Configuration["Redis:Port"]!));
builder.Services.AddSqlServer(builder.Configuration["SqlServer:Default"]!);
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();
var container = app.Services.GetAutofacRoot();
Bootstrapper.SetContainer(container);

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseExceptionHandler("/Error");
app.AddNetCoreSettings(appName);
app.Run();
