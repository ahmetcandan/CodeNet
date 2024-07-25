using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Settings;
using CodeNet.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace CodeNet.BackgroundJob.Extensions;

public static class BackgroundJobServiceExtensions
{
    /// <summary>
    /// Add Background Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="services"></param>
    /// <param name="cronExpression"></param>
    /// <param name="lockExperyTime"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddBackgroundService<TJob>(this IServiceCollection services, string cronExpression, TimeSpan? lockExperyTime = null)
        where TJob : class, IScheduleJob
    {
        if (!CronExpression.IsValidExpression(cronExpression))
            throw new ArgumentException($"CronExpression is not valid: '{cronExpression}'");

        services.Configure<BackgroundServiceOptions<TJob>>(c =>
        {
            c.CronExpression = cronExpression;
            c.ExperyTime = lockExperyTime ?? TimeSpan.FromSeconds(10);
        });
        services.AddSingleton<IScheduleJob, TJob>();
        services.AddSingleton<ICodeNetBackgroundService<TJob>, CodeNetBackgroundService<TJob>>();
        return services;
    }

    /// <summary>
    /// Background Job Use Redis
    /// </summary>
    /// <param name="services"></param>
    /// <param name="redisSection"></param>
    /// <returns></returns>
    public static IServiceCollection AddBackgroundJobRedis(this IServiceCollection services, IConfigurationSection redisSection)
    {
        services.AddRedisDistributedLock(redisSection);
        return services;
    }

    /// <summary>
    /// Use Background Job
    /// </summary>
    /// <typeparam name="TJob"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static WebApplication UseBackgroundService(this WebApplication app)
    {
        var serviceScope = app.Services.CreateScope();
        var tJobs = serviceScope.ServiceProvider.GetServices<IScheduleJob>();
        foreach ( var tJob in tJobs)
        {
            var tJobType = tJob.GetType(); 
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                await backgroundService.StartAsync(CancellationToken.None);
            });
        }
        return app;
    }
}
