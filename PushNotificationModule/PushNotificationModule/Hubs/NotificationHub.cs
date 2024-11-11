using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    public async Task SendSubscriptionUpdate(string userId, bool isSubscribed)
    {
        await Clients.All.SendAsync("ReceiveSubscriptionUpdate", userId, isSubscribed);
    }
}
