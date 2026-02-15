using Microsoft.AspNetCore.SignalR;

namespace CodeNet.BackgroundJob.Manager;

internal class StatusChangeHub : Hub
{
    public async Task SendMessage(ReceivedMessageEventArgs args) => await Clients.All.SendAsync("ReceiveMessage", args);
}
