using CodeNet.BackgroundJob.Manager;
using Cronos;

namespace CodeNet.BackgroundJob.Settings;

internal class JobOptions<TJob> : JobOptions
    where TJob : class, IScheduleJob
{
    public string ServiceType { get; set; }
    public string Title { get; set; }
    public CronExpression? Cron { get; set; }
    public TimeSpan? PeriodTime { get; set; }
}

public class JobOptions
{
    public TimeSpan? ExpryTime { get; set; }
    public TimeSpan? Timeout { get; set; }
}
