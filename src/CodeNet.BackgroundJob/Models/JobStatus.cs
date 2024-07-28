namespace CodeNet.BackgroundJob.Models;

public enum JobStatus : byte
{
    Stopped = 0,
    Running = 1,
    Pending = 2
}
