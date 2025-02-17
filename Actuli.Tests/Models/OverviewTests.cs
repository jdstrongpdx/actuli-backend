using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Tests.Models;

public class OverviewTests
{
    [Fact]
    public void Overview_CanBeConstructed()
    {
        // Act
        var overview = new Overview();

        // Assert
        Assert.NotNull(overview);
        Assert.Null(overview.Location);
        Assert.Null(overview.Education);
        Assert.Null(overview.Work);
        Assert.Null(overview.Relationships);
        Assert.Null(overview.Identity);
        Assert.Null(overview.Religion);
        Assert.Null(overview.Travel);
        Assert.Null(overview.Health);
        Assert.Null(overview.Hobbies);
        Assert.Null(overview.Giving);
        Assert.Null(overview.Finances);
        Assert.Null(overview.Housing);
        Assert.Null(overview.Goals);
        Assert.Null(overview.Achievements);
        Assert.Null(overview.Summary);
    }

    [Fact]
    public void Overview_AllProperties_CanBeSetAndRetrieved()
    {
        // Arrange
        var educationSummary = new CategorySummary
        {
            Satisfaction = "Satisfied",
            Importance = "High",
            ChangeGoalDescription = "Improve knowledge",
            ProfileSummary = "Strong educational background",
            GoalsSummary = "Achieving academic goals",
            AchievementsSummary = "Completed certifications"
        };

        var overview = new Overview
        {
            Location = "New York",
            Education = educationSummary,
            Work = new CategorySummary { Satisfaction = "Very Satisfied" },
            Goals = "Learn new skills",
            Achievements = "Earned a Master's degree",
            Summary = "Overall positive progress"
        };

        // Assert
        Assert.Equal("New York", overview.Location);
        Assert.Equal("Satisfied", overview.Education.Satisfaction);
        Assert.Equal("Very Satisfied", overview.Work.Satisfaction);
        Assert.Equal("Learn new skills", overview.Goals);
        Assert.Equal("Earned a Master's degree", overview.Achievements);
        Assert.Equal("Overall positive progress", overview.Summary);
    }

    [Fact]
    public void Overview_CanBeSerializedToJson()
    {
        // Arrange
        var overview = new Overview
        {
            Location = "Los Angeles",
            Goals = "Travel the world",
            Achievements = "Visited 25 countries",
            Education = new CategorySummary
            {
                Satisfaction = "Content",
                Importance = "Moderate",
                ChangeGoalDescription = "Advance skills",
                ProfileSummary = "Continuously learning",
                GoalsSummary = "Improving career prospects",
                AchievementsSummary = "Completed online courses"
            }
        };

        // Act
        var json = JsonConvert.SerializeObject(overview);

        // Assert
        Assert.Contains("\"location\":\"Los Angeles\"", json);
        Assert.Contains("\"goals\":\"Travel the world\"", json);
        Assert.Contains("\"achievements\":\"Visited 25 countries\"", json);
        Assert.Contains("\"satisfaction\":\"Content\"", json);
        Assert.Contains("\"importance\":\"Moderate\"", json);
        Assert.Contains("\"changeGoalDescription\":\"Advance skills\"", json);
    }

    [Fact]
    public void Overview_CanBeDeserializedFromJson()
    {
        // Arrange
        var json = @"
    {
        ""location"": ""London"",
        ""education"": {
            ""satisfaction"": ""Very High"",
            ""importance"": ""Critical"",
            ""changeGoalDescription"": ""Learn advanced skills"",
            ""profileSummary"": ""Education is a strong point"",
            ""goalsSummary"": ""Achieving higher education goals"",
            ""achievementsSummary"": ""Graduated with honors""
        },
        ""work"": {
            ""satisfaction"": ""Satisfied"",
            ""importance"": ""High"",
            ""changeGoalDescription"": ""Improve work-life balance""
        },
        ""goals"": ""Secure a leadership position"",
        ""achievements"": ""Promoted to team lead"",
        ""summary"": ""Making significant progress""
    }";

        // Act
        var overview = JsonConvert.DeserializeObject<Overview>(json);

        // Assert
        Assert.NotNull(overview);
        Assert.Equal("London", overview.Location);
        Assert.NotNull(overview.Education);
        Assert.Equal("Very High", overview.Education.Satisfaction);
        Assert.Equal("Critical", overview.Education.Importance);
        Assert.Equal("Learn advanced skills", overview.Education.ChangeGoalDescription);
        Assert.NotNull(overview.Work);
        Assert.Equal("Satisfied", overview.Work.Satisfaction);
        Assert.Equal("High", overview.Work.Importance);
        Assert.Equal("Making significant progress", overview.Summary); // Fixed expected value
        Assert.Equal("Secure a leadership position", overview.Goals);
        Assert.Equal("Promoted to team lead", overview.Achievements);
    }

    [Fact]
    public void Overview_CanHandlePartialJsonData()
    {
        // Arrange
        var json = @"
        {
            ""location"": ""Paris"",
            ""goals"": ""Learn French"",
            ""summary"": ""Focused on cultural growth""
        }";

        // Act
        var overview = JsonConvert.DeserializeObject<Overview>(json);

        // Assert
        Assert.NotNull(overview);
        Assert.Equal("Paris", overview.Location);
        Assert.Null(overview.Education); // Not provided in JSON
        Assert.Equal("Learn French", overview.Goals);
        Assert.Equal("Focused on cultural growth", overview.Summary);
    }
}
