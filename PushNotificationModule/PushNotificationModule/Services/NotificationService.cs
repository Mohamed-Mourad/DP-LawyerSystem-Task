using System;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.SignalR;

public class NotificationService
{
    private readonly NotificationDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(NotificationDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }
    
    public async Task BroadcastNotification(Notifications notification)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification.UserId, notification.Message);
        var users = _context.Users.Where(u => u.IsSubscribed).ToList();
        foreach (var user in users)
        {
            await SendNotificationToUser(user.Id, notification);
        }
    }

    
    public async Task SendNotificationToUser(int userId, Notifications notification)
    {
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);
        if(user != null)
        {
            if (user.IsConnected)
            {
                await SendNotificationToOnlineUser(user.Id, notification);
            }
            else
            {
                await StoreNotificationToOfflineUser(user.Id, notification);
            }
        }
    }


    private async Task SendNotificationToOnlineUser(int userId, Notifications notification)
    {
        await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", $"You have recieved a notification from", notification.Message);
        var newNotification = new Notifications
        {
            UserId = userId,
            Message = notification.Message,
        };

        _context.Notifications.Add(newNotification);
        await _context.SaveChangesAsync();
        
    }

    private async Task StoreNotificationToOfflineUser(int userId, Notifications notification)
    {
        var pendingNotification = new Notifications
        {
            UserId = notification.UserId,
            Message = notification.Message,
        };

        _context.PendingNotifications.Add(pendingNotification);
        await _context.SaveChangesAsync();
    }


    public async Task DeliverNotificationsForReconnectedUser(int userId)
    {
        var pendingNotifications = _context.PendingNotifications.Where(pn => pn.UserId == userId).ToList();
        if (pendingNotifications.Any())
        {
            foreach (var notification in pendingNotifications)
            {
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notification.Message);

                var newNotification = new Notifications
                {
                    Id = notification.Id,
                    UserId = userId,
                    Message = notification.Message,
                };

                _context.Notifications.Add(newNotification);
            }

            await _context.SaveChangesAsync();

            await ClearPendingNotificationsForReconnectedUser(userId);
        }
    }


    private async Task ClearPendingNotificationsForReconnectedUser(int userId)
    {
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);
        if (user != null)
        {
            var pendingNotifications = _context.PendingNotifications.Where(pn => pn.UserId == userId).ToList();
            if (pendingNotifications.Any())
            {
                _context.PendingNotifications.RemoveRange(pendingNotifications);
                await _context.SaveChangesAsync();
            }
        }
    }

    public async Task UpdateUserConnectionStatus(int userId, bool isConnected)
    {
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);        
        if (user != null)
        {
            user.IsConnected = isConnected;
            await _context.SaveChangesAsync();
        }
    }
}
