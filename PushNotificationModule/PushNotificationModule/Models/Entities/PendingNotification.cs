using System;

public class PendingNotification
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
