using Autofac;
using CodeNet.Core.Module;
using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.Identity.Api.Handler;
using CodeNet.Identity.Module;
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCodeNetContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<CodeNetModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterModule<IdentityModule>();
});

builder.AddCodeNet("Application")
       .AddAuthentication("Identity")
       .AddIdentity(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");

builder.Build()
    .UseCodeNet(builder.Configuration, "Application")
    .Run();
