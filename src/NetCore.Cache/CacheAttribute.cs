namespace NetCore.Cache;

/// <summary>
/// Cache
/// </summary>
/// <param name="time">Minutes</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method)]
public class CacheAttribute() : Attribute
{
    public required int Time { get; set; }
}
