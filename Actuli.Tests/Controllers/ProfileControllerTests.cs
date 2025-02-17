using Actuli.Api.Controllers;
using Actuli.Api.Interfaces;
using Actuli.Api.Models.ProfileTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using Actuli.Api.Models;

namespace Actuli.Tests.Controllers;

public class ProfileControllerTests
{
    private readonly Mock<IAppUserService> _mockService;
    private readonly ProfileController _controller;

    public ProfileControllerTests()
    {
        _mockService = new Mock<IAppUserService>();

        _controller = new ProfileController(_mockService.Object);

        // Mock HttpContext and User
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
    public async Task UpdateContact_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Error", "Model is invalid");

        // Act
        var result = await _controller.UpdateContact(null);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var error = Assert.IsType<SerializableError>(badRequest.Value);

        // Verify that the error contains the "Error" key and its associated message
        Assert.True(error.ContainsKey("Error"));
        Assert.Equal("Model is invalid", ((string[])error["Error"])[0]);
    }

    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenContactIsNull()
    {
        // Act
        var result = await _controller.UpdateContact(null);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Contact is null.", badRequest.Value);
    }

    [Fact]
    public async Task UpdateContact_ReturnsNotFound_WhenUserNotFound()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;
        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((AppUser)null);

        // Act
        var result = await _controller.UpdateContact(new Contact());

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User not found.", notFound.Value);
    }

    [Fact]
    public async Task UpdateContact_UpdatesFields_WhenContactFieldsProvided()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;

        var storedUser = new AppUser(userId)
        {
            Profile = new Profile
            {
                Contact = new Contact
                {
                    Email = "oldemail@example.com",
                    FirstName = "OldFirstName"
                }
            }
        };

        var updatedContact = new Contact
        {
            Email = "newemail@example.com",
            FirstName = "NewFirstName"
        };

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(storedUser);
        _mockService.Setup(s => s.UpdateUserAsync(userId, storedUser)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateContact(updatedContact);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(storedUser, okResult.Value);
        Assert.Equal("newemail@example.com", storedUser.Profile.Contact.Email);
        Assert.Equal("NewFirstName", storedUser.Profile.Contact.FirstName);

        _mockService.Verify(s => s.UpdateUserAsync(userId, storedUser), Times.Once);
    }

    [Fact]
    public async Task UpdateContact_EnsuresUnchangedFieldsRemainSame()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;

        var storedUser = new AppUser(userId)
        {
            Profile = new Profile
            {
                Contact = new Contact
                {
                    Email = "oldemail@example.com",
                    FirstName = "OldFirstName",
                    LastName = "OldLastName"
                }
            }
        };

        var updatedContact = new Contact
        {
            Email = "newemail@example.com" // Only update the email
        };

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(storedUser);
        _mockService.Setup(s => s.UpdateUserAsync(userId, storedUser)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateContact(updatedContact);

        // Assert
        Assert.Equal("newemail@example.com", storedUser.Profile.Contact.Email);
        Assert.Equal("OldFirstName", storedUser.Profile.Contact.FirstName);
        Assert.Equal("OldLastName", storedUser.Profile.Contact.LastName);
    }

    [Fact]
    public async Task UpdateContact_CallsGenerateMethods()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;

        var storedUser = new AppUser(userId)
        {
            Profile = new Profile
            {
                Contact = new Contact()
            }
        };

        var updatedContact = new Contact
        {
            Email = "test@example.com"
        };

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(storedUser);
        _mockService.Setup(s => s.UpdateUserAsync(userId, storedUser)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateContact(updatedContact);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(storedUser, okResult.Value);

        // Verify derived value generators were called
        Assert.NotNull(storedUser.Profile.Contact.GenerateAddress); // Mock these as simple stubs
        Assert.NotNull(storedUser.Profile.Contact.GenerateAge);
    }

    [Fact]
    public async Task UpdateContact_ReturnsOk_WhenUpdateIsSuccessful()
    {
        // Arrange
        var userId = _controller.ControllerContext.HttpContext.User.FindFirst("oid").Value;

        var storedUser = new AppUser(userId)
        {
            Profile = new Profile
            {
                Contact = new Contact()
            }
        };

        var updatedContact = new Contact
        {
            Email = "updated@example.com"
        };

        _mockService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(storedUser);
        _mockService.Setup(s => s.UpdateUserAsync(userId, storedUser)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateContact(updatedContact);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _mockService.Verify(s => s.UpdateUserAsync(userId, storedUser), Times.Once);
    }
}
