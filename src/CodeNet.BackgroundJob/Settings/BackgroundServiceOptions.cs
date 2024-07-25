namespace CodeNet.BackgroundJob.Settings;

public class BackgroundServiceOptions<TJob>
{
    public string CronExpression { get; internal set; }
    public TimeSpan ExperyTime { get; internal set; }
}
