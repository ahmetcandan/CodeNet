using Autofac;
using CodeNet.Container.Module;
using CodeNet.Extensions;
using CodeNet.Identity.Api.Handler;
using CodeNet.Identity.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterModule<IdentityModule>();
});
builder.AddNetCore("Application");
builder.AddAuthentication("Identity");
builder.AddIdentity("SqlServer", "Identity");

var app = builder.Build();

app.UseNetCore(builder.Configuration, "Application");
app.Run();
