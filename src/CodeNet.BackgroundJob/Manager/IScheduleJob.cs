namespace CodeNet.BackgroundJob.Manager;

public interface IScheduleJob
{
    Task Execute(CancellationToken cancellationToken);
}
