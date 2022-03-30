using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace JobPortalApi.Hubs;

public class ChatHub: Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceivedMessage", user, message);
    }
}