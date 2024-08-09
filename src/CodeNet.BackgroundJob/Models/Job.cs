namespace CodeNet.BackgroundJob.Models;

public class Job
{
    public int Id { get; set; }
    public string ServiceType { get; set; }
    public string Title { get; set; }
    public string CronExpression { get; set; }
    public TimeSpan? ExpryTime { get; set; }
    public JobStatus Status { get; set; }
    public bool IsActive { get; set; }
}
