using Microsoft.AspNetCore.SignalR;

namespace SignalrServer.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("showString", message);
        }
    }
}
