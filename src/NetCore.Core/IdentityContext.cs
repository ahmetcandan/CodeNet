using Microsoft.AspNetCore.Http;
using NetCore.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NetCore.Core
{
    public class IdentityContext : IIdentityContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            return _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        public IEnumerable<string> GetRoles()
        {
            return _httpContextAccessor?.HttpContext?.User?.Claims?.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value);
        }

        public string GetToken()
        {
            if (_httpContextAccessor?.HttpContext?.Request?.Headers?["Authorization"].Any() == true)
            {
                var authValues = _httpContextAccessor.HttpContext.Request.Headers["Authorization"][0].Split(' ');
                if (authValues?.Count() > 1)
                    return authValues[1];
            }

            return string.Empty;
        }
    }
}
