using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

public class SubscriptionServiceTests
{
    private readonly SubscriptionService _subscriptionService;
    private readonly Mock<NotificationDbContext> _dbContextMock;
    private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;

    public SubscriptionServiceTests()
    {
        _dbContextMock = new Mock<NotificationDbContext>(new DbContextOptions<NotificationDbContext>());
        _hubContextMock = new Mock<IHubContext<NotificationHub>>();
        _subscriptionService = new SubscriptionService(_dbContextMock.Object, _hubContextMock.Object);
    }

    [Fact]
    public async Task Subscribe_UserExists_ReturnsTrue()
    {
        // Arrange
        var user = new User { Id = 1, IsSubscribed = false };
        _dbContextMock.Setup(db => db.Users.SingleOrDefault(It.IsAny<Func<User, bool>>())).Returns(user);

        // Act
        var result = await _subscriptionService.Subscribe(new UserSubscriptionDto { UserId = 1, IsSubscribed = true });

        // Assert
        Assert.True(result);
        Assert.True(user.IsSubscribed);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _hubContextMock.Verify(hub => hub.Clients.All.SendAsync("ReceiveSubscriptionUpdate", "user1", true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Subscribe_UserNotFound_ReturnsFalse()
    {
        // Arrange
        _dbContextMock.Setup(db => db.Users.SingleOrDefault(It.IsAny<Func<User, bool>>())).Returns((User)null);

        // Act
        var result = await _subscriptionService.Subscribe(new UserSubscriptionDto { UserId = 1, IsSubscribed = true });

        // Assert
        Assert.False(result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _hubContextMock.Verify(hub => hub.Clients.All.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Unsubscribe_UserExists_ReturnsTrue()
    {
        // Arrange
        var user = new User { Id = 1, IsSubscribed = true };
        _dbContextMock.Setup(db => db.Users.SingleOrDefault(It.IsAny<Func<User, bool>>())).Returns(user);

        // Act
        var result = await _subscriptionService.Unsubscribe(new UserSubscriptionDto { UserId = 1, IsSubscribed = false });

        // Assert
        Assert.True(result);
        Assert.False(user.IsSubscribed);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _hubContextMock.Verify(hub => hub.Clients.All.SendAsync("ReceiveSubscriptionUpdate", "user1", false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Unsubscribe_UserNotFound_ReturnsFalse()
    {
        // Arrange
        _dbContextMock.Setup(db => db.Users.SingleOrDefault(It.IsAny<Func<User, bool>>())).Returns((User)null);

        // Act
        var result = await _subscriptionService.Unsubscribe(new UserSubscriptionDto { UserId = 1, IsSubscribed = false });

        // Assert
        Assert.False(result);
        _dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _hubContextMock.Verify(hub => hub.Clients.All.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
