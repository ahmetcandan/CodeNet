using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.Redis.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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
    public BackgroundJobOptionsBuilder AddJob<TJob>(string cronExpression, TimeSpan? lockExperyTime = null)
        where TJob : class, IScheduleJob
    {
        if (!CronExpression.IsValidExpression(cronExpression))
            throw new ArgumentException($"CronExpression is not valid: '{cronExpression}'");

        services.Configure<JobOptions<TJob>>(c =>
        {
            c.CronExpression = cronExpression;
            c.ExpryTime = lockExperyTime ?? TimeSpan.FromSeconds(10);
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
