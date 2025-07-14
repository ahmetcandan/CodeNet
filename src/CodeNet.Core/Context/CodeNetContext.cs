using CodeNet.Core.Enums;
using CodeNet.Core.Extensions;
using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace CodeNet.Core.Context;

internal class CodeNetContext(IHttpContextAccessor httpContextAccessor) : ICodeNetContext
{
    private string? _correlationId;
    public string CorrelationId
    {
        get
        {
            if (!string.IsNullOrEmpty(_correlationId))
                return _correlationId;

            var headerCorrelationId = httpContextAccessor?.HttpContext?.Request?.Headers?[Constant.CorrelationId].ToString();
            _correlationId = string.IsNullOrEmpty(headerCorrelationId)
                ? Guid.NewGuid().ToString("N")
                : headerCorrelationId;

            if (httpContextAccessor?.HttpContext is not null)
                httpContextAccessor.HttpContext.Response.Headers[Constant.CorrelationId] = _correlationId;

            return _correlationId;
        }
    }

    public string? UserName => httpContextAccessor.HttpContext?.User.Identity?.Name;

    public string? Email => httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

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

    public bool SetResponseHeader(string key, string value) => httpContextAccessor.HttpContext is not null
        && httpContextAccessor.HttpContext.Response.Headers.SetResponseHeader(key, value);

    public StringValues GetResponseHeader(string key) => httpContextAccessor.HttpContext?.Response.Headers[key] ?? new();

    public StringValues GetRequestHeader(string key) => httpContextAccessor.HttpContext?.Request.Headers[key] ?? new();

    public IHeaderDictionary? RequestHeaders => httpContextAccessor?.HttpContext?.Request.Headers;
}
