using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationsController(NotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    // Sends a notification, saving if the user is offline
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
    {
        // Check if user is connected (add logic as needed in the service)
        var isUserConnected = await _notificationService.IsUserConnected(request.userId);

        if (isUserConnected)
        {
            // Send real-time notification to the connected user
            await _hubContext.Clients.User(request.userId).SendAsync("ReceiveNotification", request.message);
            return Ok(new { Status = "Notification sent to online user" });
        }
        else
        {
            // Save notification for offline user
            await _notificationService.SaveNotificationForOfflineUser(request);
            return Ok(new { Status = "Notification saved for offline user" });
        }
    }
}
