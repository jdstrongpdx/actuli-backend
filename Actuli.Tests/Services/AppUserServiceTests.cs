using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Actuli.Api.Interfaces;
using Actuli.Api.Models;
using Actuli.Api.Services;
using Moq;
using Xunit;

namespace Actuli.Tests.Services;

public class AppUserServiceTests
{
    private readonly Mock<ICosmosDbRepository<AppUser>> _mockRepository;
    private readonly AppUserService _service;

    public AppUserServiceTests()
    {
        // Mock the repository
        _mockRepository = new Mock<ICosmosDbRepository<AppUser>>();
        // Initialize the service with the mocked repository
        _service = new AppUserService(_mockRepository.Object);
    }

    [Fact]
    public async Task AddUserAsync_ShouldCallRepositoryAddItemAsync()
    {
        // Arrange
        var user = new AppUser("1") { Name = "Test User" };
        _mockRepository
            .Setup(repo => repo.AddItemAsync(user))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AddUserAsync(user);

        // Assert
        _mockRepository.Verify(repo => repo.AddItemAsync(user), Times.Once); // Ensure AddItemAsync is called
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var userId = "1";
        var user = new AppUser(userId) { Name = "Existing User" };

        _mockRepository
            .Setup(repo => repo.GetItemAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.Equal(user, result); // Ensure returned user matches the expected user
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "2";

        _mockRepository
            .Setup(repo => repo.GetItemAsync(userId))
            .ReturnsAsync((AppUser)null); // Simulate user not found

        // Act
        var result = await _service.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result); // Ensure result is null
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnListOfUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<AppUser>
        {
            new AppUser("1") { Name = "User 1" },
            new AppUser("2") { Name = "User 2" }
        };

        _mockRepository
            .Setup(repo => repo.GetAllItemsAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _service.GetAllUsersAsync();

        // Assert
        Assert.Equal(users, result); // Ensure the returned list matches the expected list
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenNoUsersExist()
    {
        // Arrange
        _mockRepository
            .Setup(repo => repo.GetAllItemsAsync())
            .ReturnsAsync(new List<AppUser>());

        // Act
        var result = await _service.GetAllUsersAsync();

        // Assert
        Assert.Empty(result); // Ensure the result is an empty list
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldCallRepositoryUpdateItemAsync()
    {
        // Arrange
        var userId = "1";
        var user = new AppUser(userId) { Name = "Updated User" };

        _mockRepository
            .Setup(repo => repo.UpdateItemAsync(userId, user))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateUserAsync(userId, user);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateItemAsync(userId, user),
            Times.Once); // Ensure UpdateItemAsync is called
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldCallRepositoryDeleteItemAsync()
    {
        // Arrange
        var userId = "1";

        _mockRepository
            .Setup(repo => repo.DeleteItemAsync(userId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteUserAsync(userId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteItemAsync(userId), Times.Once); // Ensure DeleteItemAsync is called
    }
}