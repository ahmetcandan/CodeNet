namespace CodeNet.Cache;

/// <summary>
/// Cache
/// </summary>
/// <param name="expiryTime">Seconds, Default 30 seconds</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method)]
public class LockAttribute() : Attribute
{
    public int ExpiryTime { get; set; } = 30;
}
