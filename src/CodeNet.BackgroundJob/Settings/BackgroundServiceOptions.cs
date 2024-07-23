namespace CodeNet.BackgroundJob.Settings;

public class BackgroundServiceOptions<TJob>
{
    public string CronExpression { get; set; }
    public Func<TJob, Task> Func { get; set; }
}
