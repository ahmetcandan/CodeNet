﻿using CodeNet.BackgroundJob.Models;

namespace CodeNet.BackgroundJob.Manager;

public interface ICodeNetBackgroundService<TJob> : ICodeNetBackgroundService
    where TJob : IScheduleJob
{
}

public interface ICodeNetBackgroundService
{
    Task SetManuelTime(TimeSpan manuelTimeSpan, CancellationToken cancellationToken = default);
    Task SetManuelTime(DateTime manuelDateTime, CancellationToken cancellationToken = default);
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    Task<JobWorkingDetail?> DoWorkAsync(IScheduleJob tJob, int jobId, CancellationToken cancellationToken = default);
    JobStatus GetStatus();
    int GetJobId();
    event StatusChanged? StatusChanged;
}