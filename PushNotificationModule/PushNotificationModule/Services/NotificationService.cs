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
    private readonly FirebaseApp _firebaseApp;

    public NotificationService(NotificationDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
        _firebaseApp = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("dp-lawyersystem-task-firebase-adminsdk-5hxsw-b91df7bc84.json")
        });
    }
    
    public async Task BroadcastNotification(Notification notification)
    {
        var users = _context.Users.Where(u => u.IsSubscribed).ToList();
        foreach (var user in users)
        {
            await SendNotificationToUser(user.Id, notification);
        }
    }

    
    public async Task SendNotificationToUser(int userId, Notification notification)
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


    private async Task SendNotificationToOnlineUser(int userId, Notification notification)
    {
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);
        if (user != null)
        {
            var message = new Message
            {
                Token = user.FcmToken,
            };

            // Send the notification using Firebase Cloud Messaging
            var messaging = FirebaseMessaging.GetMessaging(_firebaseApp);
            try
            {
                var result = await messaging.SendAsync(message);

                var newNotification = new Notification
                {
                    UserId = userId,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt,
                    IsRead = false
                };

                _context.Notifications.Add(newNotification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                // Handle exception
            }
        }
    }

    private async Task StoreNotificationToOfflineUser(int userId, Notification notification)
    {
        var pendingNotification = new Notification
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            CreatedAt = notification.CreatedAt,
            IsRead = false,
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

                var newNotification = new Notification
                {
                    Id = notification.Id,
                    UserId = userId,
                    Message = notification.Message,
                    CreatedAt = notification.CreatedAt,
                    IsRead = false,
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
}
