using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpPut("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] UserSubscriptionDto subscriptionDto)
    {
        if (string.IsNullOrEmpty(subscriptionDto.UserId))
        {
            return BadRequest(new { message = "UserId is required." });
        }

        try
        {
            var result = await _subscriptionService.Subscribe(subscriptionDto);

            if (result)
            {
                return Ok(new { message = "User subscribed successfully." });
            }
            else
            {
                return NotFound(new { message = "User not found." });
            }
        }
        catch
        {
            return StatusCode(500, new { message = "An error occurred while subscribing the user." });
        }
    }

    [HttpPut("unsubscribe")]
    public async Task<IActionResult> Unsubscribe([FromBody] UserSubscriptionDto subscriptionDto)
    {
        if (string.IsNullOrEmpty(subscriptionDto.UserId))
        {
            return BadRequest(new { message = "UserId is required." });
        }

        try
        {
            var result = await _subscriptionService.Unsubscribe(subscriptionDto);

            if (result)
            {
                return Ok(new { message = "User unsubscribed successfully." });
            }
            else
            {
                return NotFound(new { message = "User not found." });
            }
        }
        catch
        {
            return StatusCode(500, new { message = "An error occurred while unsubscribing the user." });
        }
    }
}
