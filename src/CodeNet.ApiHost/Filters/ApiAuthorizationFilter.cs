using CodeNet.ApiHost.Extensions;
using CodeNet.ApiHost.Attributes;
using CodeNet.Core.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.ApiHost.Filters;

internal class ApiAuthorizationFilter : IEndpointFilter
{
    private readonly ICodeNetContext _codeNetContext;

    public ApiAuthorizationFilter(IServiceProvider service)
    {
        var scope = service.CreateScope();
        _codeNetContext = scope.ServiceProvider.GetRequiredService<ICodeNetContext>();
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var userName = _codeNetContext?.UserName;
        var authAttrs = context.HttpContext.GetEndpoint()?.Metadata.OfType<XAuthAttribute>();

        if (authAttrs?.Count() > 0)
        {
            if (string.IsNullOrEmpty(userName))
                return Results.Unauthorized();

            foreach (var attr in authAttrs)
            {
                if (!string.IsNullOrEmpty(attr.Users))
                    if (attr.Users.SemicolonSplit().Contains(userName))
                        return await next(context);

                if (!string.IsNullOrEmpty(attr.Roles))
                    if (_codeNetContext?.Roles?.Any(role => attr.Roles.SemicolonSplit().Contains(role)) is true)
                        return await next(context);
            }
        }
        else
            return await next(context);

        return Results.Unauthorized();
    }
}
