using Actuli.Api.Controllers;
using Actuli.Api.Interfaces;
using Actuli.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Actuli.Tests.Controllers;

public class AppUserControllerTests
{
    private readonly Mock<IAppUserService> _mockService;
    private readonly AppUserController _controller;

    public AppUserControllerTests()
    {
        _mockService = new Mock<IAppUserService>();

        _controller = new AppUserController(_mockService.Object);

        // Mock HttpContext with a valid "User"
        var httpContext = new DefaultHttpContext();
        httpContext.User = new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("oid", Guid.NewGuid().ToString())
            })
        );
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetAsync_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;
        var expectedAppUser = new AppUser(userId);

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(expectedAppUser);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAppUser = Assert.IsType<AppUser>(okResult.Value);
        Assert.Equal(expectedAppUser.Id, actualAppUser.Id);
        _mockService.Verify(s => s.GetUserByIdAsync(userId), Times.Once);
    }


    [Fact]
    public async Task GetAsync_CreatesUser_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        _mockService.Verify(s => s.AddUserAsync(It.IsAny<AppUser>()), Times.Once);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task PutAsync_UpdatesUser_WhenUserExists()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;
        var appUser = new AppUser(userId);

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(appUser);
        _mockService.Setup(s => s.UpdateUserAsync(userId, appUser)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.PutAsync(appUser);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(appUser, okResult.Value);
        _mockService.Verify(s => s.UpdateUserAsync(userId, appUser), Times.Once);
    }


    [Fact]
    public async Task PutAsync_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var appUser = new AppUser(userId);
        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.PutAsync(appUser);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesUser_WhenUserExists()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;
        var appUser = new AppUser(userId);

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(appUser);
        _mockService.Setup(s => s.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAsync();

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockService.Verify(s => s.DeleteUserAsync(userId), Times.Once);
    }


    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.DeleteAsync();

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
