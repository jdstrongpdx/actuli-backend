using System;
using Actuli.Api.Utilities;
using Xunit;

namespace Actuli.Tests.Utilities;

public class DateTimeUtilsTests
{
    [Fact]
    public void CalculateAge_Should_Return_Correct_Age_On_Birthday()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 2, 15);
        DateTime currentDate = new DateTime(2025, 2, 15);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(25, age);
    }

    [Fact]
    public void CalculateAge_Should_Return_Correct_Age_One_Day_Before_Birthday()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 2, 15);
        DateTime currentDate = new DateTime(2025, 2, 14);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(24, age);
    }

    [Fact]
    public void CalculateAge_Should_Handle_Leap_Year_Birthday()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 2, 29);
        DateTime currentDate = new DateTime(2025, 2, 28);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(25, age);
    }

    [Fact]
    public void CalculateAge_Should_Handle_Leap_Year_Birthday_After_February()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 2, 29);
        DateTime currentDate = new DateTime(2025, 3, 1);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(25, age);
    }

    [Fact]
    public void CalculateAge_Should_Handle_Birthdays_In_December()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 12, 31);
        DateTime currentDate = new DateTime(2025, 12, 30);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(24, age);
    }

    [Fact]
    public void CalculateAge_Should_Handle_Birthdays_Exactly_Today()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 2, 15);
        DateTime currentDate = new DateTime(2025, 2, 15);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(25, age);
    }

    [Fact]
    public void CalculateAge_Should_Handle_Birthdays_After_CurrentDate_In_Year()
    {
        // Arrange
        DateTime birthDate = new DateTime(2000, 10, 10);
        DateTime currentDate = new DateTime(2025, 5, 1);

        // Act
        int age = DateTimeUtils.CalculateAge(birthDate, currentDate);

        // Assert
        Assert.Equal(24, age);
    }
}