using CodeNet.BackgroundJob.Settings;
using CodeNet.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;
using System.Reflection;

namespace CodeNet.BackgroundJob.Manager;

internal class CodeNetBackgroundService<TJob>(TJob service,IAppLogger appLogger, IOptions<BackgroundServiceOptions<TJob>> options) : ICodeNetBackgroundService<TJob>
{
    private bool _exit = false;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cron = new CronExpression(options.Value.CronExpression);
        var methodInfo = MethodBase.GetCurrentMethod();
        while (!cancellationToken.IsCancellationRequested && !_exit)
        {
            var now = DateTimeOffset.Now;
            var nextTime = cron.GetNextValidTimeAfter(now);
            var timeSpan = nextTime - now ?? new TimeSpan(0);
            await Task.Delay(timeSpan, cancellationToken);
            try
            {
                appLogger.EntryLog($"'{nameof(TJob)}' starting scheduled task.", methodInfo);
                var timer = new Stopwatch();
                timer.Start();
                await options.Value.Func(service);
                timer.Stop();
                appLogger.ExitLog($"'{nameof(TJob)}' scheduled mission is over.", methodInfo, timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                appLogger.ExceptionLog(ex, methodInfo);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        appLogger.TraceLog($"'{nameof(TJob)}' stoped scheduled task.", MethodBase.GetCurrentMethod());
        _exit = true;
        return Task.CompletedTask;
    }
}
