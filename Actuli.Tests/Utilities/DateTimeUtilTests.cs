using System;
using Xunit;
// Make sure to reference the namespace where the CalculateAge method is defined
using Actuli.Api.Utilities;

namespace Actuli.Tests.Utilities;

public class DateTimeUtilsTests
{
    [Theory]
    [InlineData(1980, 1, 1, 2023, 1, 1, 43)] // Birthday has occurred this year
    [InlineData(1980, 12, 31, 2023, 12, 30, 42)] // Birthday has not occurred yet this year
    [InlineData(1980, 12, 31, 2023, 12, 31, 43)] // Birthday is today
    [InlineData(2025, 1, 1, 2024, 1, 1, 0)] // Birth date in the future => 0
    public void CalculateAge_WithVariousBirthDates_ReturnsExpectedAge(int birthYear, int birthMonth, int birthDay,
        int currentYear, int currentMonth, int currentDay,
        int expectedAge)
    {
        // Arrange
        var birthDate = new DateTime(birthYear, birthMonth, birthDay);
        var testDate = new DateTime(currentYear, currentMonth, currentDay);

        // Act
        var actualAge = DateTimeUtils.CalculateAge(birthDate, testDate);

        // Assert
        Assert.Equal(expectedAge, actualAge);
    }

    [Fact]
    public void CalculateAge_WithDefaultDate_UsesToday()
    {
        // Arrange
        // Here, we do not pass in a 'currentDate',
        // so it should use DateTime.Today internally.
        // For demonstration, we will just assert that
        // it does not throw and returns a non-negative integer.

        var birthDate = DateTime.Today.AddYears(-30); // e.g., today 30 years ago

        // Act
        var actualAge = DateTimeUtils.CalculateAge(birthDate);

        // Assert
        Assert.True(actualAge >= 0, "Age should be a non-negative integer.");
    }
}
