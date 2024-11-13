using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    private readonly NotificationService _notificationService;

    public NotificationHub(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override async Task OnConnectedAsync()
{
    var userIdString = Context.GetHttpContext()?.Request.Query["userId"];
    if (int.TryParse(userIdString, out int userId))
    {
        await _notificationService.UpdateUserConnectionStatus(userId, true);
        await _notificationService.DeliverNotificationsForReconnectedUser(userId);
    }

    await base.OnConnectedAsync();
}

public override async Task OnDisconnectedAsync(Exception exception)
{
    var userIdString = Context.GetHttpContext()?.Request.Query["userId"];
    if (int.TryParse(userIdString, out int userId))
    {
        await _notificationService.UpdateUserConnectionStatus(userId, false);
    }

    await base.OnDisconnectedAsync(exception);
}


    public async Task SendSubscriptionUpdate(string userId, bool isSubscribed)
    {
        await Clients.All.SendAsync("ReceiveSubscriptionUpdate", userId, isSubscribed);
    }
}