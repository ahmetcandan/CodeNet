namespace CodeNet.Lock.Server.Core;

internal class LockManager
{
    private readonly Dictionary<string, DateTime> _locks = [];
    private readonly object _syncLock = new();

    public LockResult TryAcquireLock(string key, DateTime expireDate)
    {
        lock (_syncLock)
        {
            var now = DateTime.UtcNow;

            if (_locks.TryGetValue(key, out DateTime _expireDate) && now < _expireDate)
                return new LockResult() { IsSuccess = false, ExpireDate = _expireDate, Message = "There is a lock that was previously acquired with this key." };

            _locks[key] = expireDate;

            return new LockResult() { IsSuccess = true, ExpireDate = expireDate, Message = "The lock has been successfully retrieved." };
        }
    }

    public void Cleanup()
    {
        lock (_syncLock)
        {
            var now = DateTime.UtcNow;
            var keysToRemove = _locks.Where(x => x.Value < now).Select(x => x.Key).ToList();
            foreach (var key in keysToRemove)
            {
                _locks.Remove(key);
            }
        }
    }
}