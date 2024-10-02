using CodeNet.Core.Settings;
using Microsoft.AspNetCore.Builder;

namespace CodeNet.Core.Extensions;

public class CodeNetApplicationBuilder(WebApplication app, ApplicationSettings applicationSettings)
{
    public CodeNetApplicationBuilder UseAuthentication()
    {
        app.UseAuthentication();
        return this;
    }

    public CodeNetApplicationBuilder UseAuthorization()
    {
        app.UseAuthorization();
        return this;
    }

    public CodeNetApplicationBuilder UseSwagger(string routePrefix = "swagger")
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/{routePrefix}/{applicationSettings.Version}/swagger.json", $"{applicationSettings.Title} {applicationSettings.Version}");
            options.RoutePrefix = routePrefix;
            options.DisplayRequestDuration();
        });
        return this;
    }
}
