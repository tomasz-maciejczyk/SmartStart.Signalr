using Microsoft.AspNetCore.SignalR;

namespace SignalrServer.Hubs
{
    public class TimeHub : Hub
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
