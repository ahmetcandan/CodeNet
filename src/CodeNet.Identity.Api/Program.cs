using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.EntityFramework.Extensions;
using CodeNet.ExceptionHandling.Extensions;
using CodeNet.Identity.Extensions;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")!);
}, SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
builder.Services.AddCodeNet(builder.Configuration.GetSection("Application"), options =>
{
    options.AddAuthentication(SecurityKeyType.AsymmetricKey, builder.Configuration.GetSection("Identity"));
});
builder.Services
    .AddRedisDistributedCache(builder.Configuration.GetSection("Redis"))
    .AddRedisDistributedLock(builder.Configuration.GetSection("Redis"))
    .AddAppLogger();

var app = builder.Build();
app.UseCodeNet(options =>
{
    options.UseSwagger();
    options.UseAuthentication();
    options.UseAuthorization();
});
app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseExceptionHandling();
app.UseLogging();
app.UseDistributedCache();
app.UseDistributedLock();
app.Run();
