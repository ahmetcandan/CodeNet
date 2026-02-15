using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.SignalR.Extensions;

public static class SignalRServiceExtensions
{
    public static IServiceCollection AddSignalRNotification(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }

    public static WebApplication UseSignalR<THub>(this WebApplication app, string pattern)
        where THub : Hub
    {
        app.UseCors();
        app.UseRouting();
        app.MapHub<THub>(pattern);
        return app;
    }
}
