using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddCodeNet("Application")
       .AddAuthenticationWithAsymmetricKey("Identity")
       .AddIdentityWithAsymmetricKey(options => options.UseSqlServer(builder.Configuration, "SqlServer"), "Identity");

builder.Build()
    .UseCodeNet(builder.Configuration, "Application")
    .Run();
