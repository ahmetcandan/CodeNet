using Autofac;
using CodeNet.Container.Module;
using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.Identity.Api.Handler;
using CodeNet.Identity.Module;
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNetCoreContainer(containerBuilder =>
{
    containerBuilder.RegisterModule<CodeNetModule>();
    containerBuilder.RegisterModule<MediatRModule<GenerateTokenRequestHandler>>();
    containerBuilder.RegisterModule<IdentityModule>();
});

builder.AddNetCore("Application")
       .AddAuthentication("Identity")
       .AddIdentity(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");

builder.Build()
    .UseNetCore(builder.Configuration, "Application")
    .Run();
