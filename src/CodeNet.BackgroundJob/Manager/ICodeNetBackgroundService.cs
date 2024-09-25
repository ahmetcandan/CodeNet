using CodeNet.BackgroundJob.Models;

namespace CodeNet.BackgroundJob.Manager;

public interface ICodeNetBackgroundService<TJob> : ICodeNetBackgroundService
    where TJob : IScheduleJob
{
}

public interface ICodeNetBackgroundService
{
    void SetManuelTime(TimeSpan manuelTimeSpan);
    void SetManuelTime(DateTime manuelDateTime);
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    Task<JobWorkingDetail?> DoWorkAsync(IScheduleJob tJob, int jobId, CancellationToken cancellationToken = default);
    JobStatus GetStatus();
    int GetJobId();
    event StatusChanged? StatusChanged;
}