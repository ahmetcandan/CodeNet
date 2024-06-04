using Autofac;
using Microsoft.AspNetCore.Identity;
using NetCore.Abstraction.Model;
using NetCore.Container;
using NetCore.Core.Extensions;
using NetCore.Identity.DbContext;
using NetCore.Identity.Handler;
using NetCore.Identity.Manager;
using NetCore.Identity.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder => 
{
    containerBuilder.RegisterModule<NetCoreModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterType<IdentityTokenManager>().As<IIdentityTokenManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityUserManager>().As<IIdentityUserManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityRoleManager>().As<IIdentityRoleManager>().InstancePerLifetimeScope();
});

var applicationSettings = builder.Configuration.GetSection("Application").Get<ApplicationSettings>()!;
builder.AddNetCore("Application");
builder.AddSqlServer<ApplicationDbContext>("SqlServer");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddAuthentication("JWT", "public_key.pem");
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));

var app = builder.Build();

app.UseNetCore(builder.Configuration, "Application");
app.Run();
