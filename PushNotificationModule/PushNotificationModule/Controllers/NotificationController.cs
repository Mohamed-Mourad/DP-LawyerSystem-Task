using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    
    [HttpPost("broadcast")]
    public async Task<IActionResult> BroadcastNotification([FromBody] Notification notification)
    {
        if (notification == null || string.IsNullOrWhiteSpace(notification.Message))
        {
            return BadRequest(new { message = "Invalid notification data." });
        }

        try
        {
            await _notificationService.BroadcastNotification(notification);
            return Ok(new { message = "Notification broadcasted successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while broadcasting the notification." });
        }
    }

    
    [HttpPost("send")]
    public async Task<IActionResult> SendMessageToUser([FromQuery] int userId, [FromBody] Notification notification)
    {
        if (userId <= 0 || notification == null || string.IsNullOrWhiteSpace(notification.Message))
        {
            return BadRequest(new { message = "Invalid request data." });
        }

        try
        {
            await _notificationService.SendNotificationToUser(userId, notification);
            return Ok(new { message = "Message sent to user successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while sending the message." });
        }
    }

    [HttpPut("deliver")]
    public async Task<IActionResult> DeliverPendingNotifications([FromQuery] int userId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "Invalid user ID." });
        }

        try
        {
            await _notificationService.DeliverNotificationsForReconnectedUser(userId);
            return Ok(new { message = "Pending notifications sent to user successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while delivering pending notifications." });
        }
    }
}
