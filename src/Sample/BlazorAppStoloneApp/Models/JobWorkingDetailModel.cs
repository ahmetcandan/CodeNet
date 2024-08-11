namespace CodeNetUI_Example.Models;

public class JobWorkingDetailModel
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string Message { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public TimeSpan? ElapsedTime { get; set; }
    public DetailStatus Status { get; set; }
}
