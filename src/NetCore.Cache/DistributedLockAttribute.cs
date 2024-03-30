namespace NetCore.Cache;

/// <summary>
/// Cache
/// </summary>
/// <param name="expiryTime">Seconds, Default 30 seconds</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method)]
public class DistributedLockAttribute() : Attribute
{
    public int ExpiryTime { get; set; } = 30;
}
