namespace CodeNet.Redis.Attributes;

/// <summary>
/// Cache
/// </summary>
/// <param name="time">Minutes</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute(int time) : Attribute
{
    /// <summary>
    /// Minutes
    /// </summary>
    public int Time { get; set; } = time;
}
