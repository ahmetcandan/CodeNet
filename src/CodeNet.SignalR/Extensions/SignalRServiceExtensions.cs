using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

namespace CodeNet.SignalR.Extensions;

public static class SignalRServiceExtensions
{
    public static HubEndpointConventionBuilder UseSignalR<THub>(this WebApplication app, string pattern)
        where THub : Hub
    {
        return app.MapHub<THub>(pattern);
    }
    public static HubEndpointConventionBuilder UseSignalR<THub>(this WebApplication app, string pattern, Action<HttpConnectionDispatcherOptions> configureOptions)
        where THub : Hub
    {
        return app.MapHub<THub>(pattern, configureOptions);
    }
}
