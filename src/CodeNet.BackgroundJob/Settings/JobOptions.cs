namespace CodeNet.BackgroundJob.Settings;

internal class JobOptions<TJob>
{
    public string CronExpression { get; internal set; }
    public TimeSpan ExpryTime { get; internal set; }
}
