using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.EntityFramework.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
       .AddAuthenticationWithAsymmetricKey(builder.Configuration.GetSection("Identity"))
       .AddIdentityWithAsymmetricKey(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!), builder.Configuration.GetSection("Identity"));

builder.Build()
    .UseCodeNet(builder.Configuration, "Application")
    .Run();
