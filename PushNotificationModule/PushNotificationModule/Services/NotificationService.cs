using System.Collections.Concurrent;
using System.Threading.Tasks;

public class NotificationService
{

    // A thread-safe dictionary to track user connections
    private static readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

    // Adds a user's connection ID to the dictionary when they connect
    public Task OnUserConnected(string userId, string connectionId)
    {
        _userConnections[userId] = connectionId;  // Save connection ID for the user
        return Task.CompletedTask;
    }

    // Removes a user's connection ID from the dictionary when they disconnect
    public Task OnUserDisconnected(string userId)
    {
        _userConnections.TryRemove(userId, out _);  // Remove user from the dictionary
        return Task.CompletedTask;
    }

    // Checks if a user is connected by their User ID
    public Task<bool> IsUserConnected(string userId)
    {
        // Check if the user has an active connection ID in the dictionary
        bool isConnected = _userConnections.ContainsKey(userId);
        return Task.FromResult(isConnected);
    }

    // Save user subscription to database
    public async Task SaveSubscription(SubscriptionDto subscription)
    {
        // TODO: save subscription information to the database
    }

    // Save notification for offline users
    public async Task SaveNotificationForOfflineUser(NotificationRequestDto request)
    {
        // TODO: save notifications for offline users
    }
}
