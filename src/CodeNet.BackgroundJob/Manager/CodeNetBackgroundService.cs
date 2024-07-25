using CodeNet.BackgroundJob.Models;
using CodeNet.BackgroundJob.Settings;
using CodeNet.EntityFramework.Repositories;
using CodeNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using RedLockNet;
using System.Diagnostics;
using System.Reflection;

namespace CodeNet.BackgroundJob.Manager;

internal class CodeNetBackgroundService<TJob>(IOptions<JobOptions<TJob>> options, IServiceProvider serviceProvider) : ICodeNetBackgroundService<TJob>
    where TJob : IScheduleJob
{
    private bool _exit = false;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cron = new CronExpression(options.Value.CronExpression);
        var methodInfo = MethodBase.GetCurrentMethod();
        int jobId;
        using var serviceScope = serviceProvider.CreateAsyncScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
        var serviceRepository = new Repository<Job>(dbContext);
        var currentJob = await serviceRepository.GetAsync(c => c.ServiceName == typeof(TJob).ToString(), cancellationToken);
        if (currentJob is null)
        {
            var jobEntity = await serviceRepository.AddAsync(new Job
            {
                CronExpression = options.Value.CronExpression,
                ExpryTime = options.Value.ExpryTime,
                ServiceName = typeof(TJob).ToString(),
                Status = JobStatus.Running
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            jobId = jobEntity.Id;
        }
        else
        {
            currentJob.Status = JobStatus.Running;
            currentJob.ExpryTime = options.Value.ExpryTime;
            currentJob.CronExpression = options.Value.CronExpression;
            serviceRepository.Update(currentJob);
            await dbContext.SaveChangesAsync(cancellationToken);
            jobId = currentJob.Id;
        }

        var distributedLock = serviceScope.ServiceProvider.GetService<IDistributedLockFactory>();
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        var tJob = serviceScope.ServiceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().Equals(typeof(TJob)));
        if (tJob is null)
            return;

        while (!cancellationToken.IsCancellationRequested && !_exit)
        {
            var workingDetailRepository = new Repository<JobWorkingDetail>(dbContext);
            var now = DateTimeOffset.Now;
            var nextTime = cron.GetNextValidTimeAfter(now);
            var timeSpan = nextTime - now ?? new TimeSpan(0);
            await Task.Delay(timeSpan, cancellationToken);
            var result = await DoWork(tJob, distributedLock, appLogger, methodInfo, cancellationToken);

            if (result is not null)
            {
                result.JobId = jobId;
                await workingDetailRepository.AddAsync(result, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            if (result is { Status: DetailStatus.Fail or DetailStatus.Stopped })
                break;
        }
    }

    private async Task<JobWorkingDetail?> DoWork(IScheduleJob tJob, IDistributedLockFactory? distributedLock, IAppLogger? appLogger, MethodBase? methodInfo, CancellationToken cancellationToken = default)
    {
        DateTimeOffset? startDate = null;
        try
        {
            if (distributedLock is not null)
            {
                using var redLock = await distributedLock.CreateLockAsync($"CNBJ_{typeof(TJob)}", options.Value.ExpryTime);
                if (!redLock.IsAcquired)
                    return null;
            }

            if (_exit)
                return new JobWorkingDetail
                {
                    Message = "Not working. This Job has been stopped.",
                    Status = DetailStatus.Stopped
                };

            appLogger?.EntryLog($"'{typeof(TJob)}' starting scheduled task.", methodInfo);
            var timer = new Stopwatch();
            timer.Start();
            startDate = DateTimeOffset.Now;
            await tJob.Execute(cancellationToken);
            timer.Stop();
            appLogger?.ExitLog($"'{typeof(TJob)}' scheduled mission is over.", methodInfo, timer.ElapsedMilliseconds);
            return new JobWorkingDetail
            {
                Message = $"Job worked successfully.",
                Status = DetailStatus.Successful,
                StartDate = startDate,
                ElapsedTime = TimeSpan.FromMicroseconds(timer.ElapsedMilliseconds)
            };
        }
        catch (Exception ex)
        {
            appLogger?.ExceptionLog(ex, methodInfo);
            return new JobWorkingDetail
            {
                Message = $"Thrown exception while Job was working. Exeption: {ex.Message}",
                Status = DetailStatus.Fail,
                StartDate = startDate
            };
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<BackgroundJobDbContext>();
        var serviceRepository = new Repository<Job>(dbContext);
        var service = await serviceRepository.GetAsync(c => c.ServiceName == typeof(TJob).ToString(), cancellationToken);
        if (service is not null)
        {
            service.Status = JobStatus.Stopped;
            serviceRepository.Update(service);
            await serviceRepository.SaveChangesAsync(cancellationToken);
        }
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        appLogger?.TraceLog($"'{nameof(TJob)}' stoped scheduled task.", MethodBase.GetCurrentMethod());
        _exit = true;
    }
}
