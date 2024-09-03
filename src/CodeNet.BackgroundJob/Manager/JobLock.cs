using RedLockNet;

namespace CodeNet.BackgroundJob.Manager;

internal class JobLock(IDistributedLockFactory distributedLockFactory) : IJobLock
{
    public Task<IRedLock> CreateLock(string jobName, TimeSpan lockTime)
    {
        return distributedLockFactory.CreateLockAsync($"CNBJ_{jobName}", lockTime);
    }
}
