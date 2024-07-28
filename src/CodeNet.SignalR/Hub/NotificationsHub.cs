using Microsoft.AspNetCore.SignalR;

namespace CodeNet.SignalR;

internal class NotificationsHub : Hub
{
    public Task SendNotification(string method, object content, CancellationToken cancellationToken)
    {
        return Clients.All.SendAsync(method, content, cancellationToken);
    }
}
