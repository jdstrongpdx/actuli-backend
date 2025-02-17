using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Tests.Models;

public class AccomplishmentTests
{
    [Fact]
    public void Accomplishment_CanBeConstructed()
    {
        // Act
        var accomplishment = new Accomplishment();

        // Assert
        Assert.NotNull(accomplishment);
        Assert.Null(accomplishment.Id);
        Assert.Null(accomplishment.GoalId);
        Assert.Equal(default, accomplishment.completedAt); // DateTime default value
        Assert.Null(accomplishment.Description);
        Assert.Null(accomplishment.Notes);
    }

    [Fact]
    public void Accomplishment_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var accomplishment = new Accomplishment
        {
            Id = "test-accomplishment-id",
            GoalId = "test-goal-id",
            completedAt = new DateTime(2023, 11, 1, 12, 0, 0),
            Description = "Completed the task successfully.",
            Notes = "This was a high-priority accomplishment."
        };

        // Assert
        Assert.Equal("test-accomplishment-id", accomplishment.Id);
        Assert.Equal("test-goal-id", accomplishment.GoalId);
        Assert.Equal(new DateTime(2023, 11, 1, 12, 0, 0), accomplishment.completedAt);
        Assert.Equal("Completed the task successfully.", accomplishment.Description);
        Assert.Equal("This was a high-priority accomplishment.", accomplishment.Notes);
    }

    [Fact]
    public void Accomplishment_CanBeSerializedToJson()
    {
        // Arrange
        var accomplishment = new Accomplishment
        {
            Id = "test-accomplishment-id",
            GoalId = "test-goal-id",
            completedAt = new DateTime(2023, 11, 1, 12, 0, 0),
            Description = "Completed the task successfully.",
            Notes = "This was a high-priority accomplishment."
        };

        // Act
        var json = JsonConvert.SerializeObject(accomplishment);

        // Assert
        Assert.Contains("\"accomplishmentId\":\"test-accomplishment-id\"", json);
        Assert.Contains("\"goalId\":\"test-goal-id\"", json);
        Assert.Contains("\"completedAt\":\"2023-11-01T12:00:00\"", json); // ISO 8601 format
        Assert.Contains("\"description\":\"Completed the task successfully.\"", json);
        Assert.Contains("\"notes\":\"This was a high-priority accomplishment.\"", json);
    }

    [Fact]
    public void Accomplishment_CanBeDeserializedFromJson()
    {
        // Arrange
        var json = @"
        {
            ""accomplishmentId"": ""test-accomplishment-id"",
            ""goalId"": ""test-goal-id"",
            ""completedAt"": ""2023-11-01T12:00:00"",
            ""description"": ""Completed the task successfully."",
            ""notes"": ""This was a high-priority accomplishment.""
        }";

        // Act
        var accomplishment = JsonConvert.DeserializeObject<Accomplishment>(json);

        // Assert
        Assert.NotNull(accomplishment);
        Assert.Equal("test-accomplishment-id", accomplishment.Id);
        Assert.Equal("test-goal-id", accomplishment.GoalId);
        Assert.Equal(new DateTime(2023, 11, 1, 12, 0, 0), accomplishment.completedAt);
        Assert.Equal("Completed the task successfully.", accomplishment.Description);
        Assert.Equal("This was a high-priority accomplishment.", accomplishment.Notes);
    }

}