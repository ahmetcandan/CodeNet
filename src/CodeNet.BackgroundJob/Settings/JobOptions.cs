using CodeNet.BackgroundJob.Manager;
using Cronos;

namespace CodeNet.BackgroundJob.Settings;

internal class JobOptions<TJob> : JobOptions
    where TJob : class, IScheduleJob
{
    public JobOptions(TimeSpan lockExpryTime) : base(lockExpryTime) { }

    public JobOptions(TimeSpan lockExpryTime, TimeSpan timeOut) : base(lockExpryTime, timeOut) { }

    public required string ServiceType { get; set; }
    public required string Title { get; set; }
    public CronExpression? Cron { get; set; }
    public TimeSpan? PeriodTime { get; set; }
}

public class JobOptions(TimeSpan lockExpryTime)
{
    public JobOptions(TimeSpan lockExpryTime, TimeSpan timeOut) : this(lockExpryTime) => TimeOut = timeOut;

    public TimeSpan? LockExpryTime { get; set; } = lockExpryTime;
    public TimeSpan? TimeOut { get; set; }
}
