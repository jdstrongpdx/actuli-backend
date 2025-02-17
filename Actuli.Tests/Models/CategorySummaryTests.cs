using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Tests.Models;

public class CategorySummaryTests
{
    [Fact]
    public void CategorySummary_CanBeConstructed()
    {
        // Act
        var categorySummary = new CategorySummary();

        // Assert
        Assert.NotNull(categorySummary);
        Assert.Null(categorySummary.Satisfaction);
        Assert.Null(categorySummary.Importance);
        Assert.Null(categorySummary.ChangeGoalDescription);
        Assert.Null(categorySummary.ProfileSummary);
        Assert.Null(categorySummary.GoalsSummary);
        Assert.Null(categorySummary.AchievementsSummary);
    }

    [Fact]
    public void CategorySummary_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var categorySummary = new CategorySummary
        {
            Satisfaction = "Very Satisfied",
            Importance = "High",
            ChangeGoalDescription = "Achieve better work-life balance",
            ProfileSummary = "Profile is well-optimized",
            GoalsSummary = "Goals are progressing well",
            AchievementsSummary = "Completed 10 major milestones"
        };

        // Assert
        Assert.Equal("Very Satisfied", categorySummary.Satisfaction);
        Assert.Equal("High", categorySummary.Importance);
        Assert.Equal("Achieve better work-life balance", categorySummary.ChangeGoalDescription);
        Assert.Equal("Profile is well-optimized", categorySummary.ProfileSummary);
        Assert.Equal("Goals are progressing well", categorySummary.GoalsSummary);
        Assert.Equal("Completed 10 major milestones", categorySummary.AchievementsSummary);
    }

    [Fact]
    public void CategorySummary_CanBeSerializedToJson()
    {
        // Arrange
        var categorySummary = new CategorySummary
        {
            Satisfaction = "Satisfied",
            Importance = "Medium",
            ChangeGoalDescription = "Focus on personal growth",
            ProfileSummary = "Summary of the profile",
            GoalsSummary = "Summary of the goals",
            AchievementsSummary = "Summary of achievements"
        };

        // Act
        var json = JsonConvert.SerializeObject(categorySummary);

        // Assert
        Assert.Contains("\"satisfaction\":\"Satisfied\"", json);
        Assert.Contains("\"importance\":\"Medium\"", json);
        Assert.Contains("\"changeGoalDescription\":\"Focus on personal growth\"", json);
        Assert.Contains("\"profileSummary\":\"Summary of the profile\"", json);
        Assert.Contains("\"goalsSummary\":\"Summary of the goals\"", json);
        Assert.Contains("\"achievementsSummary\":\"Summary of achievements\"", json);
    }

    [Fact]
    public void CategorySummary_CanBeDeserializedFromJson()
    {
        // Arrange
        var json = @"
        {
            ""satisfaction"": ""Neutral"",
            ""importance"": ""Low"",
            ""changeGoalDescription"": ""Make small daily improvements"",
            ""profileSummary"": ""Minimal profile summary"",
            ""goalsSummary"": ""Few goals in progress"",
            ""achievementsSummary"": ""Some achievements recorded""
        }";

        // Act
        var categorySummary = JsonConvert.DeserializeObject<CategorySummary>(json);

        // Assert
        Assert.NotNull(categorySummary);
        Assert.Equal("Neutral", categorySummary.Satisfaction);
        Assert.Equal("Low", categorySummary.Importance);
        Assert.Equal("Make small daily improvements", categorySummary.ChangeGoalDescription);
        Assert.Equal("Minimal profile summary", categorySummary.ProfileSummary);
        Assert.Equal("Few goals in progress", categorySummary.GoalsSummary);
        Assert.Equal("Some achievements recorded", categorySummary.AchievementsSummary);
    }
}
