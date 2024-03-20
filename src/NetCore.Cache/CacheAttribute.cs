namespace NetCore.Cache;

/// <summary>
/// Cache
/// </summary>
/// <param name="time">Seconds</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute(int time) : Attribute
{
    public int Time { get; set; } = time;
}
