using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public SubscriptionController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    // API to subscribe/unsubscribe users
    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] SubscriptionDto subscription)
    {
        await _notificationService.SaveSubscription(subscription);
        return Ok(new { Status = "Subscribed" });
    }

    // API to save notifications for offline users
    [HttpPost("saveNotification")]
    public async Task<IActionResult> SaveNotification([FromBody] NotificationRequestDto request)
    {
        await _notificationService.SaveNotificationForOfflineUser(request);
        return Ok(new { Status = "Notification saved for offline user" });
    }
}
