using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using NetCore.Abstraction.Model;
using NetCore.Container;
using NetCore.Core.Extension;
using NetCore.Identity.DbContext;
using NetCore.Identity.Handler;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<NetCoreModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();

    containerBuilder.RegisterType<IdentityTokenManager>().As<IIdentityTokenManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityUserManager>().As<IIdentityUserManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityRoleManager>().As<IIdentityRoleManager>().InstancePerLifetimeScope();
});

var applicationSettings = builder.Configuration.GetSection("Application").Get<ApplicationSettings>()!;
builder.Services.AddNetCore(applicationSettings);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration.GetConnectionString("SqlServer")!);
builder.Services.AddRedisSettings(builder.Configuration["Redis:Hostname"]!, int.Parse(builder.Configuration["Redis:Port"]!));
builder.Services.AddAuthentication(builder.Configuration["JWT:ValidAudience"]!, builder.Configuration["JWT:ValidIssuer"]!, "public_key.pem");
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));

var app = builder.Build();

app.UseNetCore(applicationSettings);
app.Run();
