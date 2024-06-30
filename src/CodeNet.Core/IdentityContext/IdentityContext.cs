using CodeNet.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeNet.Core;

internal class IdentityContext(IHttpContextAccessor HttpContextAccessor) : IIdentityContext
{
    private Guid? _requestId;
    public Guid CorrelationId
    {
        get
        {
            if (_requestId.HasValue)
                return _requestId.Value;

            var headerRequestId = HttpContextAccessor?.HttpContext?.Request?.Headers?["RequestId"].ToString();
            _requestId = !string.IsNullOrEmpty(headerRequestId) && Guid.TryParse(headerRequestId, out var requestId) ? requestId : Guid.NewGuid();

            if (HttpContextAccessor?.HttpContext is not null)
                HttpContextAccessor.HttpContext.Response.Headers["RequestId"] = _requestId.Value.ToString();

            return _requestId.Value;
        }
    }

    public string? UserName => HttpContextAccessor?.HttpContext?.User?.Identity?.Name;

    public string? Email => HttpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

    public string? UserId => HttpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    public IEnumerable<string> Roles => HttpContextAccessor?.HttpContext?.User?.Claims?.Where(c => c.Type is ClaimTypes.Role)?.Select(c => c.Value);

    public string? Token
    {
        get
        {
            if (HttpContextAccessor.HttpContext?.Request.Headers.ContainsKey("Authorization") is true)
            {
                var authValues = HttpContextAccessor.HttpContext.Request.Headers.Authorization[0]!.Split(' ');
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

            var states = HttpContextAccessor?.HttpContext?.Request?.Headers?["Cache-State"];
            if (states.HasValue)
            {
                var values = states.Value.ToString().Replace(" ", "").Split(',');
                if (values.Contains(nameof(CacheState.NoCache)))
                    cacheState |= CacheState.NoCache;
                if (values.Contains(nameof(CacheState.ClearCache)))
                    cacheState |= CacheState.ClearCache;
            }

            return cacheState;
        }
    }
}
