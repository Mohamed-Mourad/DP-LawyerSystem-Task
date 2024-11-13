using Microsoft.EntityFrameworkCore;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Notifications> Notifications { get; set; }
    public DbSet<Notifications> PendingNotifications { get; set; }
    public DbSet<User> Users { get; set; }
}
