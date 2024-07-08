using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Repository;
using StokTakip.Campaign.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"))
    .AddAuthenticationWithAsymmetricKey(builder.Configuration.GetSection("JWT"))
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddDbContext<CampaignDbContext>("SqlServer")
    .AddAppLogger()
    .AddScoped<ICampaignRepository, CampaignRepository>()
    .AddScoped<ICampaignService, CampaignService>();

var app = builder.Build();
app.UseLogging();
app.UseCodeNet(builder.Configuration.GetSection("Application"));
app.Run();
