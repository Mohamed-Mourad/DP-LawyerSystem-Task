using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class SubscriptionControllerTests
{
    private readonly SubscriptionController _controller;
    private readonly Mock<SubscriptionService> _subscriptionServiceMock;

    public SubscriptionControllerTests()
    {
        _subscriptionServiceMock = new Mock<SubscriptionService>();
        _controller = new SubscriptionController(_subscriptionServiceMock.Object);
    }

    [Fact]
    public async Task Subscribe_ValidUserId_ReturnsOk()
    {
        // Arrange
        var subscriptionDto = new UserSubscriptionDto { UserId = "user1", IsSubscribed = true };
        _subscriptionServiceMock.Setup(service => service.Subscribe(subscriptionDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Subscribe(subscriptionDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("User subscribed successfully.", ((dynamic)okResult.Value).message);
    }

    [Fact]
    public async Task Subscribe_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var subscriptionDto = new UserSubscriptionDto { UserId = "user1", IsSubscribed = true };
        _subscriptionServiceMock.Setup(service => service.Subscribe(subscriptionDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Subscribe(subscriptionDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("User not found.", ((dynamic)notFoundResult.Value).message);
    }

    [Fact]
    public async Task Unsubscribe_ValidUserId_ReturnsOk()
    {
        // Arrange
        var subscriptionDto = new UserSubscriptionDto { UserId = "user1", IsSubscribed = false };
        _subscriptionServiceMock.Setup(service => service.Unsubscribe(subscriptionDto)).ReturnsAsync(true);

        // Act
        var result = await _controller.Unsubscribe(subscriptionDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal("User unsubscribed successfully.", ((dynamic)okResult.Value).message);
    }

    [Fact]
    public async Task Unsubscribe_UserNotFound_ReturnsNotFound()
    {
        // Arrange
        var subscriptionDto = new UserSubscriptionDto { UserId = "user1", IsSubscribed = false };
        _subscriptionServiceMock.Setup(service => service.Unsubscribe(subscriptionDto)).ReturnsAsync(false);

        // Act
        var result = await _controller.Unsubscribe(subscriptionDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("User not found.", ((dynamic)notFoundResult.Value).message);
    }
}
