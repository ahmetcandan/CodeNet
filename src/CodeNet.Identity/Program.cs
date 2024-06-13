using Autofac;
using Microsoft.AspNetCore.Identity;
using CodeNet.Abstraction.Model;
using CodeNet.Container.Module;
using CodeNet.Extensions;
using CodeNet.Identity.DbContext;
using CodeNet.Identity.Handler;
using CodeNet.Identity.Manager;
using CodeNet.Identity.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterType<IdentityTokenManager>().As<IIdentityTokenManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityUserManager>().As<IIdentityUserManager>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<IdentityRoleManager>().As<IIdentityRoleManager>().InstancePerLifetimeScope();
});
var applicationSettings = builder.Configuration.GetSection("Application").Get<ApplicationSettings>()!;
builder.AddNetCore("Application");
builder.AddSqlServer<ApplicationDbContext>("SqlServer");
builder.AddAuthentication("JWT", "public_key.pem");
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JWT"));

var app = builder.Build();

app.UseNetCore(builder.Configuration, "Application");
app.Run();
