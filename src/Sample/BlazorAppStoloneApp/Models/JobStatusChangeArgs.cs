namespace BlazorAppStoloneApp.Models;

public class JobStatusChangeArgs
{
    public JobStatus Status { get; set; }
    public string ServiceName { get; set; }
    public JobWorkingDetailModel? Detail { get; set; }
    public int JobId { get; set; }
}
