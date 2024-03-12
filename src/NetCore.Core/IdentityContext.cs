using Microsoft.AspNetCore.Http;
using NetCore.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetCore.Core;

public class IdentityContext(IHttpContextAccessor httpContextAccessor) : IIdentityContext
{
    public string GetUserName()
    {
        return httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    }

    public IEnumerable<string> GetRoles()
    {
        return httpContextAccessor?.HttpContext?.User?.Claims?.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value);
    }

    public string GetToken()
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
