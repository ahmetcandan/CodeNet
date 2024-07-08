using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using StokTakip.Campaign.Abstraction.Repository;
using StokTakip.Campaign.Abstraction.Service;
using StokTakip.Campaign.Repository;
using StokTakip.Campaign.Service;

var builder = WebApplication.CreateBuilder(args);
builder.AddCodeNet("Application");
builder.AddAuthenticationWithAsymmetricKey("JWT");
builder.AddRedisDistributedCache("Redis");
builder.AddRedisDistributedLock("Redis");
builder.AddDbContext<CampaignDbContext>("SqlServer");
builder.AddLogging();
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignService, CampaignService>();

var app = builder.Build();
app.UseCodeNet(builder.Configuration, "Application");
app.Run();
