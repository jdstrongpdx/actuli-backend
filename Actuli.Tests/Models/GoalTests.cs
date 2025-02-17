using System.ComponentModel.DataAnnotations;
using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Tests.Models;

public class GoalTests
{
    [Fact]
    public void Goal_CanBeConstructed()
    {
        // Act
        var goal = new Goal
        {
            Id = "goal-id-123",
            Owner = "owner-id-456",
            Description = "This is a sample goal description."
        };

        // Assert
        Assert.NotNull(goal);
        Assert.Equal("goal-id-123", goal.Id);
        Assert.Equal("owner-id-456", goal.Owner);
        Assert.Equal("This is a sample goal description.", goal.Description);
    }

    [Fact]
    public void Goal_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var goal = new Goal
        {
            Id = "test-id",
            Owner = "test-owner",
            Description = "Test description."
        };

        // Assert
        Assert.Equal("test-id", goal.Id);
        Assert.Equal("test-owner", goal.Owner);
        Assert.Equal("Test description.", goal.Description);
    }

    [Fact]
    public void Goal_CanBeSerializedToJson()
    {
        // Arrange
        var goal = new Goal
        {
            Id = "goal123",
            Owner = "owner456",
            Description = "Test goal description"
        };

        // Act
        var json = JsonConvert.SerializeObject(goal);

        // Assert
        Assert.Contains("\"id\":\"goal123\"", json);
        Assert.Contains("\"owner\":\"owner456\"", json);
        Assert.Contains("\"description\":\"Test goal description\"", json);
    }

    [Fact]
    public void Goal_CanBeDeserializedFromJson()
    {
        // Arrange
        var json = @"
        {
            ""id"": ""goal-id-123"",
            ""owner"": ""owner-id-456"",
            ""description"": ""This is a detailed description of the goal.""
        }";

        // Act
        var goal = JsonConvert.DeserializeObject<Goal>(json);

        // Assert
        Assert.NotNull(goal);
        Assert.Equal("goal-id-123", goal.Id);
        Assert.Equal("owner-id-456", goal.Owner);
        Assert.Equal("This is a detailed description of the goal.", goal.Description);
    }

    [Fact]
    public void Goal_Validation_ShouldThrowException_When_PropertyRequired_IsMissing()
    {
        // Arrange
        var goal = new Goal
        {
            Owner = null, // Required property deliberately left invalid
            Id = null, // Required property deliberately left invalid
            Description = "Valid description only."
        };

        var validationContext = new ValidationContext(goal);

        // Act and Assert
        var exception = Assert.Throws<ValidationException>(() =>
            Validator.ValidateObject(goal, validationContext, validateAllProperties: true));
        Assert.Contains("required", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
