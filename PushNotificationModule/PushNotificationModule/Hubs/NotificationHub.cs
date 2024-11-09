using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class NotificationHub : Hub
{
    private readonly NotificationService _notificationService;

    public NotificationHub(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override Task OnConnectedAsync()
    {
        // When a user connects, add them to the connection tracking service
        var userId = Context.UserIdentifier;  // Assuming you're using user identifier to track users
        _notificationService.OnUserConnected(userId, Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(System.Exception exception)
    {
        // When a user disconnects, remove them from the connection tracking service
        var userId = Context.UserIdentifier;
        _notificationService.OnUserDisconnected(userId);
        return base.OnDisconnectedAsync(exception);
    }
    
    // Sends a notification to all connected clients
    public async Task SendNotificationToAll(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    // Sends a notification to a specific client by connection ID
    public async Task SendNotificationToUser(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
    }

    // Sends a notification to a specific group of clients
    public async Task SendNotificationToGroup(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveNotification", message);
    }
    
    // Allows clients to join a group for group-based notifications
    public Task JoinGroup(string groupName)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    // Allows clients to leave a group
    public Task LeaveGroup(string groupName)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
