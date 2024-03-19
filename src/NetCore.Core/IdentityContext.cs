using Microsoft.AspNetCore.Http;
using NetCore.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetCore.Core;

public class IdentityContext(IHttpContextAccessor httpContextAccessor) : IIdentityContext
{
    private Guid? _requestId;
    public Guid RequestId
    {
        get
        {
            if (_requestId.HasValue)
                return _requestId.Value;

            var headerRequestId = httpContextAccessor?.HttpContext?.Request?.Headers?["RequestId"].ToString();
            if (!string.IsNullOrEmpty(headerRequestId) && Guid.TryParse(headerRequestId, out var requestId))
                _requestId = requestId;
            else
                _requestId = Guid.NewGuid();

            httpContextAccessor.HttpContext.Response.Headers["RequestId"] = _requestId.Value.ToString();
            return _requestId.Value;
        }
    }

    public string UserName
    {
        get
        {
            return httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }
    }

    public string Email
    {
        get
        {
            return httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }

    public string UserId
    {
        get
        {
            return httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }

    public IEnumerable<string> Roles
    {
        get
        {
            return httpContextAccessor?.HttpContext?.User?.Claims?.Where(c => c.Type is ClaimTypes.Role)?.Select(c => c.Value);
        }
    }

    public string Token
    {
        get
        {
            if (httpContextAccessor?.HttpContext?.Request?.Headers?["Authorization"].Count != 0 == true)
            {
                var authValues = httpContextAccessor.HttpContext.Request.Headers["Authorization"][0].Split(' ');
                if (authValues?.Length > 1)
                    return authValues[1];
            }

            return string.Empty;
        }
    }
}
