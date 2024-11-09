using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<PendingNotification> PendingNotifications { get; set; }
    public DbSet<User> Users { get; set; }
}