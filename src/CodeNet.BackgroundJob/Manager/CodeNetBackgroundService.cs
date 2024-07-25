using CodeNet.BackgroundJob.Settings;
using CodeNet.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using RedLockNet;
using System.Diagnostics;
using System.Reflection;

namespace CodeNet.BackgroundJob.Manager;

internal class CodeNetBackgroundService<TJob>(IOptions<BackgroundServiceOptions<TJob>> options, IServiceProvider serviceProvider) : ICodeNetBackgroundService<TJob>
    where TJob : IScheduleJob
{
    private bool _exit = false;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cron = new CronExpression(options.Value.CronExpression);
        var methodInfo = MethodBase.GetCurrentMethod();
        var serviceScope = serviceProvider.CreateScope();
        var distributedLock = serviceScope.ServiceProvider.GetService<IDistributedLockFactory>();
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        var tJob = serviceScope.ServiceProvider.GetServices<IScheduleJob>().FirstOrDefault(c => c.GetType().Equals(typeof(TJob)));
        if (tJob is null)
            return;

        while (!cancellationToken.IsCancellationRequested && !_exit)
        {
            var now = DateTimeOffset.Now;
            var nextTime = cron.GetNextValidTimeAfter(now);
            var timeSpan = nextTime - now ?? new TimeSpan(0);
            await Task.Delay(timeSpan, cancellationToken);
            try
            {
                if (distributedLock is not null)
                {
                    using var redLock = await distributedLock.CreateLockAsync($"CNBJ_{typeof(TJob)}", options.Value.ExperyTime);
                    if (!redLock.IsAcquired)
                        continue;
                }

                appLogger?.EntryLog($"'{nameof(TJob)}' starting scheduled task.", methodInfo);
                var timer = new Stopwatch();
                timer.Start();
                await tJob.Execute(cancellationToken);
                timer.Stop();
                appLogger?.ExitLog($"'{nameof(TJob)}' scheduled mission is over.", methodInfo, timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                appLogger?.ExceptionLog(ex, methodInfo);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        var serviceScope = serviceProvider.CreateScope();
        var appLogger = serviceScope.ServiceProvider.GetService<IAppLogger>();
        appLogger?.TraceLog($"'{nameof(TJob)}' stoped scheduled task.", MethodBase.GetCurrentMethod());
        _exit = true;
        return Task.CompletedTask;
    }
}
