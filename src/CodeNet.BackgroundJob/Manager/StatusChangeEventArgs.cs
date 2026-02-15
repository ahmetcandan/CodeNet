using CodeNet.BackgroundJob.Models;

namespace CodeNet.BackgroundJob.Manager;

public class ReceivedMessageEventArgs : EventArgs
{
    public JobStatus Status { get; set; }
    public required string ServiceName { get; set; }
    public JobWorkingDetail? Detail { get; set; }
    public int JobId { get; set; }
}
