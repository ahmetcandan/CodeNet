namespace CodeNet.Outbox.Settings;

public class OutboxSettings
{
    public TimeSpan SendPeriod { get; set; } = TimeSpan.FromMinutes(1);
    public int PrefetchCount { get; set; } = 100;
    public OutboxLockSettings? LockSettings { get; set; }
}

public class OutboxLockSettings
{
    public TimeSpan? LockTime { get; set; }
    public TimeSpan? TimeOut { get; set; }
}