using CodeNet.BackgroundJob.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;

namespace CodeNet.BackgroundJob.Middleware;

internal class BasicAuthMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string _authorizationHeader = "Authorization";
    private const string _basicScheme = "Basic";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(_authorizationHeader, out Microsoft.Extensions.Primitives.StringValues value))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var authHeader = value.ToString();
        if (authHeader.StartsWith(_basicScheme, StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader[_basicScheme.Length..].Trim();
            try
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                var options = context.RequestServices.GetService<IOptions<JobAuthOptions>>();
                if (options?.Value.BasicAuthOptions?.UserPass.Contains(KeyValuePair.Create(username, password)) is true)
                {
                    await _next(context);
                    return;
                }
            }
            catch (FormatException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid Base64 string format.");
                return;
            }
        }

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.Headers.WWWAuthenticate = "Basic realm=\"dotnet-example\"";
    }
}
