using Microsoft.AspNetCore.SignalR;

namespace ReindeerGuidance.Core.SignalR
{
    public class IncidentsHub: Hub
    {
        public void BroadcastMessage(string name, string message)
        {
            Clients.All.SendAsync("broadcast", name, message);
        }

        public void Echo(string name, string message)
        {
            Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
        }
    }
}
