using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class NotificationControllerTests
{
    private readonly NotificationController _controller;
    private readonly Mock<NotificationService> _mockNotificationService;

    public NotificationControllerTests()
    {
        _mockNotificationService = new Mock<NotificationService>();
        _controller = new NotificationController(_mockNotificationService.Object);
    }

    [Fact]
    public async Task BroadcastNotification_ReturnsBadRequest_WhenNotificationIsNull()
    {
        // Act
        var result = await _controller.BroadcastNotification(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid notification data.", ((dynamic)badRequestResult.Value).message);
    }

    [Fact]
    public async Task BroadcastNotification_ReturnsOk_WhenNotificationIsValid()
    {
        // Arrange
        var notification = new Notifications { Message = "Test message" };

        _mockNotificationService
            .Setup(s => s.BroadcastNotification(notification))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.BroadcastNotification(notification);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Notification broadcasted successfully.", ((dynamic)okResult.Value).message);
    }

    [Fact]
    public async Task SendMessageToUser_ReturnsBadRequest_WhenUserIdIsInvalid()
    {
        // Act
        var result = await _controller.SendMessageToUser(0, new Notifications { Message = "Test" });

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid request data.", ((dynamic)badRequestResult.Value).message);
    }

    [Fact]
    public async Task SendMessageToUser_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var notification = new Notifications { Message = "Test message" };
        int userId = 1;

        _mockNotificationService
            .Setup(s => s.SendNotificationToUser(userId, notification))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SendMessageToUser(userId, notification);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Message sent to user successfully.", ((dynamic)okResult.Value).message);
    }

    [Fact]
    public async Task DeliverPendingNotifications_ReturnsBadRequest_WhenUserIdIsInvalid()
    {
        // Act
        var result = await _controller.DeliverPendingNotifications(0);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid user ID.", ((dynamic)badRequestResult.Value).message);
    }

    [Fact]
    public async Task DeliverPendingNotifications_ReturnsOk_WhenUserIdIsValid()
    {
        // Arrange
        int userId = 1;

        _mockNotificationService
            .Setup(s => s.DeliverNotificationsForReconnectedUser(userId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeliverPendingNotifications(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Pending notifications sent to user successfully.", ((dynamic)okResult.Value).message);
    }
}
