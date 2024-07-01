using CodeNet.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace CodeNet.Core;

internal class CodeNetHttpContext(IHttpContextAccessor httpContextAccessor) : ICodeNetHttpContext
{
    private Guid? _correlationId;
    public Guid CorrelationId
    {
        get
        {
            if (_correlationId.HasValue)
                return _correlationId.Value;

            var headerCorrelationId = httpContextAccessor?.HttpContext?.Request?.Headers?["CorrelationId"].ToString();
            _correlationId = !string.IsNullOrEmpty(headerCorrelationId) && Guid.TryParse(headerCorrelationId, out var correlationId) ? correlationId : Guid.NewGuid();

            if (httpContextAccessor?.HttpContext is not null)
                httpContextAccessor.HttpContext.Response.Headers["CorrelationId"] = _correlationId.Value.ToString();

            return _correlationId.Value;
        }
    }

    public string? UserName => httpContextAccessor.HttpContext?.User.Identity?.Name;

    public string? Email => httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    public string? UserId => httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    public IEnumerable<string> Roles => httpContextAccessor.HttpContext?.User.Claims?.Where(c => c.Type is ClaimTypes.Role).Select(c => c.Value) ?? [];

    public string? Token
    {
        get
        {
            if (httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("Authorization") is true)
            {
                var authValues = httpContextAccessor.HttpContext.Request.Headers.Authorization[0]!.Split(' ');
                if (authValues?.Length > 1)
                    return authValues[1];
            }

            return null;
        }
    }

    public CacheState CacheState
    {
        get
        {
            CacheState cacheState = CacheState.None;

            var states = httpContextAccessor.HttpContext?.Request?.Headers?.CacheControl;
            if (states.HasValue)
            {
                var values = states.Value.ToString().Replace(" ", "").ToLower().Split(',');
                if (values.Contains("no-cache"))
                    cacheState |= CacheState.NoCache;
                if (values.Contains("clear-cache"))
                    cacheState |= CacheState.ClearCache;
            }

            return cacheState;
        }
    }

    public bool SetResponseHeader(string key, string value)
    {
        var x = httpContextAccessor.HttpContext?.User.Identity?.Name;
        StringValues strings = new();
        if (httpContextAccessor.HttpContext?.Response.Headers.TryGetValue(key, out strings) == true)
            httpContextAccessor.HttpContext.Response.Headers.Remove(key);

        return httpContextAccessor.HttpContext?.Response.Headers.TryAdd(key, new StringValues([.. strings, value])) ?? false;
    }

    public StringValues GetResponseHeader(string key)
    {
        return httpContextAccessor.HttpContext?.Response.Headers[key] ?? new();
    }

    public StringValues GetRequestHeader(string key)
    {
        return httpContextAccessor.HttpContext?.Request.Headers[key] ?? new();
    }
}
