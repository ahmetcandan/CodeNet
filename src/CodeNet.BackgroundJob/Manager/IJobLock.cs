using RedLockNet;

namespace CodeNet.BackgroundJob.Manager;

public interface IJobLock
{
    Task<IRedLock> CreateLock(string jobName, TimeSpan lockTime);
}
