namespace CodeNet.BackgroundJob.Models;

public class JobWorkingDetail
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public required string Message { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public TimeSpan? ElapsedTime { get; set; }
    public DetailStatus Status { get; set; }
}
