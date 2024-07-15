using CodeNet.Core.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Core.Enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
       .AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"))
       .AddAuthorization(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!), SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));

builder.Build()
    .UseCodeNet()
    .Run();
