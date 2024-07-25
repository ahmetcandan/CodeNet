namespace CodeNet.BackgroundJob.Manager;

public interface ICodeNetBackgroundService<TJob> : ICodeNetBackgroundService
    where TJob : IScheduleJob
{
}

public interface ICodeNetBackgroundService
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}