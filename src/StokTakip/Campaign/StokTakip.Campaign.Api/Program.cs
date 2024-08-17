using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using Microsoft.EntityFrameworkCore;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Repository;
using StokTakip.Campaign.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"), options => 
        {
            options.AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("JWT"));
        })
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddDbContext<CampaignDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!))
    .AddAppLogger()
    .AddScoped<ICampaignRepository, CampaignRepository>()
    .AddScoped<ICampaignService, CampaignService>();

var app = builder.Build();
app.UseLogging();
app.UseCodeNet();
app.Run();
