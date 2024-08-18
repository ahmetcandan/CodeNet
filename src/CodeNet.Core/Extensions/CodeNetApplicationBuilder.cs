using Microsoft.AspNetCore.Builder;

namespace CodeNet.Core.Extensions;

public class CodeNetApplicationBuilder(WebApplication app)
{
    public CodeNetApplicationBuilder UseAuthentication()
    {
        app.UseAuthentication();
        return this;
    }
}
