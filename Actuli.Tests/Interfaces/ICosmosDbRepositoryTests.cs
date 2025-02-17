using Moq;
using Actuli.Api.Interfaces;

namespace Actuli.Tests.Repositories;

public class TestModel
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}

public class ICosmosDbRepositoryTests
{
    [Fact]
    public async Task AddItemAsync_ShouldInvokeRepositoryMethodWithCorrectParameters()
    {
        // Arrange
        var mockRepository = new Moq.Mock<ICosmosDbRepository<TestModel>>();
        var expectedItem = new TestModel
        {
            Id = "test-id",
            Name = "Test"
        };

        // Act
        await mockRepository.Object.AddItemAsync(expectedItem);

        // Assert
        // Verify that AddItemAsync was called exactly once 
        // with the same object that has the Id "test-id".
        mockRepository.Verify(repo => repo.AddItemAsync(
            It.Is<TestModel>(m => m.Id == "test-id")), Times.Once);
    }

    [Fact]
    public async Task GetItemAsync_ShouldReturnCorrectItem()
    {
        // Arrange
        var mockRepository = new Moq.Mock<ICosmosDbRepository<TestModel>>();
        var expectedItem = new TestModel
        {
            Id = "test-id",
            Name = "Test"
        };

        // Set up the mock to return "expectedItem" anytime GetItemAsync is called with "test-id"
        mockRepository.Setup(repo => repo.GetItemAsync("test-id"))
            .ReturnsAsync(expectedItem);

        // Act
        var actual = await mockRepository.Object.GetItemAsync("test-id");

        // Assert
        Assert.Equal(expectedItem, actual);
        mockRepository.Verify(repo => repo.GetItemAsync("test-id"), Times.Once);
    }

    [Fact]
    public async Task GetAllItemsAsync_ShouldReturnExpectedItems()
    {
        // Arrange
        var mockRepository = new Moq.Mock<ICosmosDbRepository<TestModel>>();
        var expectedItems = new List<TestModel>
        {
            new TestModel { Id = "id1", Name = "Item1" },
            new TestModel { Id = "id2", Name = "Item2" }
        };

        // Mock the repository to return a list of items
        mockRepository.Setup(repo => repo.GetAllItemsAsync())
            .ReturnsAsync(expectedItems);

        // Act
        var actualItems = await mockRepository.Object.GetAllItemsAsync();

        // Assert
        Assert.Equal(expectedItems, actualItems);
        mockRepository.Verify(repo => repo.GetAllItemsAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateItemAsync_ShouldInvokeRepositoryMethodWithCorrectParameters()
    {
        // Arrange
        var mockRepository = new Moq.Mock<ICosmosDbRepository<TestModel>>();
        var itemToUpdate = new TestModel
        {
            Id = "test-id",
            Name = "Updated"
        };

        // Act
        await mockRepository.Object.UpdateItemAsync(itemToUpdate.Id, itemToUpdate);

        // Assert
        mockRepository.Verify(repo => repo.UpdateItemAsync("test-id", itemToUpdate),
            Times.Once);
    }

    [Fact]
    public async Task DeleteItemAsync_ShouldInvokeRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Moq.Mock<ICosmosDbRepository<TestModel>>();
        var testId = "test-id";

        // Act
        await mockRepository.Object.DeleteItemAsync(testId);

        // Assert
        mockRepository.Verify(repo => repo.DeleteItemAsync("test-id"), Times.Once);
    }
}
