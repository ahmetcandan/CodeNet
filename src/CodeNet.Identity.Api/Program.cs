using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Identity.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"), options => 
            {
                options.AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
            })
       .AddAuthorization(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!), SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));

builder.Build()
    .UseCodeNet(options => 
    {
        options.UseAuthentication();
        options.UseAuthorization();
    })
    .Run();
