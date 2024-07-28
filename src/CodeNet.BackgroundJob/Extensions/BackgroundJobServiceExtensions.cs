using CodeNet.BackgroundJob.Manager;
using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.EntityFramework.Extensions;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Logging.Extensions;
using CodeNet.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.IO;

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
    /// <param name="optionsAction">If null then uses InMemoryDatabase</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddBackgroundJob<TJob>(this IServiceCollection services, string cronExpression, TimeSpan? lockExperyTime = null)
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
        return services;
    }

    /// <summary>
    /// Add BackgroundJob DbContext
    /// </summary>
    /// <param name="services"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddBackgroundServiceDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddAppLogger();
        return services.AddDbContext<BackgroundJobDbContext>(optionsAction);
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
    public static WebApplication UseBackgroundService(this WebApplication app, string path)
    {
        var serviceScope = app.Services.CreateScope();
        app.UseEndPoint(serviceScope, path);

        var tJobs = serviceScope.ServiceProvider.GetServices<IScheduleJob>();
        foreach (var tJob in tJobs)
        {
            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");
            app.Lifetime.ApplicationStarted.Register(async () => await backgroundService.StartAsync(CancellationToken.None));
        }
        return app;
    }

    private static WebApplication UseEndPoint(this WebApplication app, IServiceScope serviceScope, string path)
    {
        app.MapGet($"{path}/getServices", async (int page, int count, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            return await serviceRepository.GetPagingListAsync(page, count, cancellationToken);
        });

        app.MapGet($"{path}/getServiceDetails", async (int jobId, DetailStatus? status, int page, int count, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
            var jobWorkingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
            return await jobWorkingDetailRepository.GetQueryable(c => c.JobId == jobId && (status == null || c.Status == status))
                .OrderByDescending(c => c.StartDate)
                .Skip((page - 1) * count).Take(count)
                .ToListAsync(cancellationToken);
        });

        app.MapDelete($"{path}/deleteDetails", async ([FromBody] int[] detailIds, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
            var jobWorkingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
            var list = await jobWorkingDetailRepository.FindAsync(c => detailIds.Any(i => c.Id == i), cancellationToken);
            jobWorkingDetailRepository.RemoveRange(list);
            return await dbContext.SaveChangesAsync(cancellationToken);
        });

        app.MapPut($"{path}/changeStatus", async (int jobId, [FromBody] JobStatus status, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            var jobEntity = await serviceRepository.GetAsync([jobId], cancellationToken) ?? throw new Exception("Job entity is not found!");
            var tJob = serviceScope.ServiceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().ToString() == jobEntity.ServiceName) ?? throw new Exception($"{jobEntity.ServiceName} IScheduleJob is not found!");

            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");

            var currentStatus = backgroundService.GetStatus();
            switch (status)
            {
                case JobStatus.Stopped:
                    if (currentStatus == status)
                        return $"The {jobEntity.ServiceName} service is already stopped.";

                    await backgroundService.StopAsync(cancellationToken);
                    return $"{jobEntity.ServiceName} service has been stopped.";
                case JobStatus.Running:
                    if (currentStatus == status)
                        return $"The {jobEntity.ServiceName} service is already running.";

                    app.Lifetime.ApplicationStarted.Register(async () => await backgroundService.StartAsync(CancellationToken.None));
                    return $"{jobEntity.ServiceName} service has been started.";
                default:
                    throw new NotImplementedException();
            }
        });

        app.MapGet($"{path}/getJobStatus", async (int jobId, CancellationToken cancellationToken) =>
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
            var serviceRepository = new Repository<Job>(dbContext);
            var jobEntity = await serviceRepository.GetAsync([jobId], cancellationToken) ?? throw new Exception("Job entity is not found!");
            var tJob = serviceScope.ServiceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().ToString() == jobEntity.ServiceName) ?? throw new Exception($"{jobEntity.ServiceName} IScheduleJob is not found!");

            var tJobType = tJob.GetType();
            var serviceType = typeof(ICodeNetBackgroundService<>).MakeGenericType(tJobType);
            var backgroundService = serviceScope.ServiceProvider.GetService(serviceType) as ICodeNetBackgroundService ?? throw new NotImplementedException($"'builder.services.AddBackgroundService<{tJobType.Name}>(string cronExpression, TimeSpan? lockExperyTime = null)' not implemented background service.");

            var currentStatus = backgroundService.GetStatus();
            return new
            {
                Status = (int)currentStatus,
                StatusName = currentStatus.ToString()
            };
        });

        return app;
    }
}
