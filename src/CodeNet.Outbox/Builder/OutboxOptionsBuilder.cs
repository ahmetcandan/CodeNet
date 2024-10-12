using CodeNet.MongoDB.Extensions;
using CodeNet.MongoDB.Settings;
using CodeNet.Outbox.Manager;
using CodeNet.Outbox.Models;
using CodeNet.Outbox.Repositories;
using CodeNet.Redis.Extensions;
using CodeNet.Redis.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Outbox.Builder;

public class OutboxOptionsBuilder
{
    private readonly IServiceCollection _services;

    public OutboxOptionsBuilder(IServiceCollection services)
    {
        _services = services;
        _services.AddScoped<OutboxRepository>();
        _services.AddScoped<IOutboxService, OutboxService>();
    }

    /// <summary>
    /// Add MongoDB
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public OutboxOptionsBuilder AddMongoDB(IConfigurationSection configuration)
    {
        var options = configuration.Get<MongoDbOptions>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json");
        return AddMongoDB(options);
    }

    /// <summary>
    /// Add MongoDB
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public OutboxOptionsBuilder AddMongoDB(MongoDbOptions options)
    {
        _services.AddMongoDB(new MongoDbOptions<OutboxDbContext>
        {
            ConnectionString = options.ConnectionString,
            DatabaseName = options.DatabaseName
        });
        return this;
    }

    /// <summary>
    /// Add Redis for Distributed Lock
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public OutboxOptionsBuilder AddRedis(IConfigurationSection configuration)
    {
        var options = configuration.Get<RedisSettings>() ?? throw new ArgumentNullException($"'{configuration.Path}' is null or empty in appSettings.json");
        return AddRedis(options);
    }

    /// <summary>
    /// Add Redis for Distributed Lock
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public OutboxOptionsBuilder AddRedis(RedisSettings settings)
    {
        _services.AddRedisDistributedLock(settings);
        return this;
    }
}
