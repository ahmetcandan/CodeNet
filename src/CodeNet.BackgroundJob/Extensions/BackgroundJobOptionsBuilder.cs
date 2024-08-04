using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.Redis.Extensions;
using Cronos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.BackgroundJob.Extensions;

public class BackgroundJobOptionsBuilder(IServiceCollection services)
{
    /// <summary>
    /// Add Redis for Lock
    /// </summary>
    /// <param name="redisSection"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddRedis(IConfigurationSection redisSection)
    {
        services.AddRedisDistributedLock(redisSection);
        return this;
    }

    /// <summary>
    /// Add Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="cronExpression"></param>
    /// <param name="lockExperyTime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public BackgroundJobOptionsBuilder AddJob<TJob>(JobOptions options)
        where TJob : class, IScheduleJob
    {
        var valid = CronExpression.TryParse(options.CronExpression, out CronExpression cron);
        if (!valid)
            throw new ArgumentException($"CronExpression is not valid: '{options.CronExpression}'");

        services.Configure<JobOptions<TJob>>(c =>
        {
            c.CronExpression = options.CronExpression;
            c.Cron = cron;
            c.ExpryTime = options.ExpryTime;
            c.Timeout = options.Timeout;
        });

        services.AddSingleton<IScheduleJob, TJob>();
        services.AddSingleton<ICodeNetBackgroundService<TJob>, CodeNetBackgroundService<TJob>>();
        return this;
    }

    /// <summary>
    /// Add DbContext
    /// </summary>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public BackgroundJobOptionsBuilder AddDbContext(Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<BackgroundJobDbContext>(optionsAction);
        return this;
    }
}
