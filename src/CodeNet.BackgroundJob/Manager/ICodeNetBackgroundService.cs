namespace CodeNet.BackgroundJob.Manager;

public interface ICodeNetBackgroundService<TJob>
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}