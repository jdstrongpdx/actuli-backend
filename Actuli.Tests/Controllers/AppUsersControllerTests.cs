using System.Collections.Generic;
using System.Threading.Tasks;
using Actuli.Api.Controllers;
using Actuli.Api.Interfaces;
using Actuli.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Actuli.Tests.Controllers;

public class AppUsersControllerTests
{
    private readonly Mock<IAppUserService> _mockService;
    private readonly AppUsersController _controller;

    public AppUsersControllerTests()
    {
        _mockService = new Mock<IAppUserService>();
        _controller = new AppUsersController(_mockService.Object);
    }

    [Fact]
    public async Task CreateItem_CallsAddUserAsync_AndReturnsOk()
    {
        // Arrange
        var appUser = new AppUser("1")
        {
            Username = "testuser",
            Name = "Test User"
        };

        // Act
        var result = await _controller.CreateItem("1", appUser);

        // Assert
        _mockService.Verify(s => s.AddUserAsync(appUser), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetItem_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetUserByIdAsync("1")).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.GetItem("1");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetItem_ReturnsOk_WithUser_WhenUserExists()
    {
        // Arrange
        var appUser = new AppUser("1")
        {
            Username = "testuser",
            Name = "Test User",
            Profile = new Profile(),
            Overview = new Overview()
        };

        _mockService.Setup(s => s.GetUserByIdAsync("1")).ReturnsAsync(appUser);

        // Act
        var result = await _controller.GetItem("1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(appUser, okResult.Value);
        _mockService.Verify(s => s.GetUserByIdAsync("1"), Times.Once);
    }

    [Fact]
    public async Task GetAllItems_ReturnsOk_WithListOfUsers()
    {
        // Arrange
        var users = new List<AppUser>
        {
            new AppUser("1") { Username = "user1", Name = "User One" },
            new AppUser("2") { Username = "user2", Name = "User Two" }
        };

        _mockService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllItems();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(users, okResult.Value);
        _mockService.Verify(s => s.GetAllUsersAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateItem_CallsUpdateUserAsync_AndReturnsOk()
    {
        // Arrange
        var appUser = new AppUser("1")
        {
            Username = "updateduser",
            Name = "Updated User"
        };

        // Act
        var result = await _controller.UpdateItem("1", appUser);

        // Assert
        _mockService.Verify(s => s.UpdateUserAsync("1", appUser), Times.Once);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteItem_CallsDeleteUserAsync_AndReturnsNoContent()
    {
        // Act
        var result = await _controller.DeleteItem("1");

        // Assert
        _mockService.Verify(s => s.DeleteUserAsync("1"), Times.Once);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteItem_DoesNotThrow_WhenUserDoesNotExist()
    {
        // Arrange
        // Simulate that the user doesn't exist by not setting up anything special for `DeleteUserAsync`

        // Act
        var result = await _controller.DeleteItem("nonexistent-user");

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockService.Verify(s => s.DeleteUserAsync("nonexistent-user"), Times.Once);
    }
}
