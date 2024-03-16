using System;

namespace NetCore.Cache;

/// <summary>
/// Cache
/// </summary>
/// <param name="time">Seconds</param>
/// <remarks>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class CacheAttribute(int time = 60) : Attribute
{
    public int Time { get; set; } = time;
}
