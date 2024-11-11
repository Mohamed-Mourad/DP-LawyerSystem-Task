using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class SubscriptionService
{
    private readonly NotificationDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public SubscriptionService(NotificationDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<bool> Subscribe(UserSubscriptionDto subscriptionDto)
    {
        var user = _context.Users.SingleOrDefault(u => u.UserId == subscriptionDto.UserId);

        if (user != null)
        {
            user.IsSubscribed = true;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveSubscriptionUpdate", subscriptionDto.UserId, subscriptionDto.IsSubscribed);
            return true;
        }
        return false;
    }

    public async Task<bool> Unsubscribe(UserSubscriptionDto subscriptionDto)
    {
        var user = _context.Users.SingleOrDefault(u => u.UserId == subscriptionDto.UserId);

        if (user != null)
        {
            user.IsSubscribed = false;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveSubscriptionUpdate", subscriptionDto.UserId, subscriptionDto.IsSubscribed);
            return true;
        }
        return false;
    }
}
