using CodeNet.Redis.Attributes;

namespace CodeNet.Parameters.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal class ParameterCacheAttribute : CacheAttribute
{
    public ParameterCacheAttribute() : base(360) { }

    public ParameterCacheAttribute(int time) : base(360) => Time = time;
}
