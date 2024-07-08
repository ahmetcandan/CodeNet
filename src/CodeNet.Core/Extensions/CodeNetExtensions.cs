using CodeNet.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CodeNet.Core.Extensions;

public static class CodeNetExtensions
{

    public static bool SetResponseHeader(this IHeaderDictionary headers, string key, string value)
    {
        if (headers.TryGetValue(key, out StringValues strings) is true)
            headers.Remove(key);

        return headers.TryAdd(key, new StringValues([.. strings, value]));
    }

    public static CacheState GetCacheState(this IHeaderDictionary headers)
    {
        CacheState cacheState = CacheState.None;

        var states = headers.CacheControl;
        var values = states.ToString().Replace(" ", "").ToLower().Split(',');
        if (values.Contains("no-cache"))
            cacheState |= CacheState.NoCache;
        if (values.Contains("clear-cache"))
            cacheState |= CacheState.ClearCache;

        return cacheState;
    }
}
