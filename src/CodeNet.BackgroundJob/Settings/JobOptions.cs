using CodeNet.BackgroundJob.Manager;
using Cronos;

namespace CodeNet.BackgroundJob.Settings;

public class JobOptions<TJob> : JobOptions
    where TJob : class, IScheduleJob
{
    public string ServiceType { get; set; }
    public string Title { get; set; }
}

public class JobOptions
{
    public string CronExpression { get; set; }
    internal CronExpression Cron { get; set; }
    public TimeSpan? ExpryTime { get; set; }
    public TimeSpan? Timeout { get; set; }
}
