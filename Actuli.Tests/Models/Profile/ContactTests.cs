using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Xunit;
using Actuli.Api.Models;

namespace Actuli.Tests.Models.Profile;

public class ContactTests
{
    [Fact]
    public void CreateContact_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange & Act
        var contact = new Contact
        {
            Username = "testuser",
            Email = "test.email@example.com",
            FirstName = "John",
            LastName = "Doe",
            Street = "123 Main St",
            City = "Sample City",
            State = "Sample State",
            PostalCode = "12345",
            Country = "Sample Country",
            DateOfBirth = new DateTime(1985, 5, 15)
        };

        // Assert
        Assert.Equal("testuser", contact.Username);
        Assert.Equal("test.email@example.com", contact.Email);
        Assert.Equal("John", contact.FirstName);
        Assert.Equal("Doe", contact.LastName);
        Assert.Equal("123 Main St", contact.Street);
        Assert.Equal("Sample City", contact.City);
        Assert.Equal("Sample State", contact.State);
        Assert.Equal("12345", contact.PostalCode);
        Assert.Equal("Sample Country", contact.Country);
        Assert.Equal(new DateTime(1985, 5, 15), contact.DateOfBirth);
    }

    [Fact]
    public void ValidateEmail_ShouldFailForInvalidEmail()
    {
        // Arrange
        var contact = new Contact
        {
            Email = "invalid-email" // Invalid email
        };

        var validationContext = new ValidationContext(contact)
        {
            MemberName = nameof(contact.Email)
        };

        // Act
        var results = new System.Collections.Generic.List<ValidationResult>();
        var isValid = Validator.TryValidateProperty(contact.Email, validationContext, results);

        // Assert
        Assert.False(isValid);
        Assert.Single(results);
        Assert.Equal("Invalid email address", results[0].ErrorMessage);
    }

    [Fact]
    public void ValidateEmail_ShouldPassForValidEmail()
    {
        // Arrange
        var contact = new Contact
        {
            Email = "valid.email@example.com" // Valid email
        };

        var validationContext = new ValidationContext(contact)
        {
            MemberName = nameof(contact.Email)
        };

        // Act
        var results = new System.Collections.Generic.List<ValidationResult>();
        var isValid = Validator.TryValidateProperty(contact.Email, validationContext, results);

        // Assert
        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void SerializeAndDeserialize_ShouldWorkCorrectly_WithSystemTextJson()
    {
        // Arrange
        var contact = new Contact
        {
            Username = "testuser",
            Email = "test.email@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var json = JsonSerializer.Serialize(contact); // Serialize object to JSON
        var deserializedContact = JsonSerializer.Deserialize<Contact>(json); // Deserialize JSON back to object

        // Assert
        Assert.NotNull(deserializedContact);
        Assert.Equal("testuser", deserializedContact.Username);
        Assert.Equal("test.email@example.com", deserializedContact.Email);
        Assert.Equal("John", deserializedContact.FirstName);
        Assert.Equal("Doe", deserializedContact.LastName);
    }

    [Fact]
    public void AddressProperty_ShouldReturnCorrectlyGeneratedAddress()
    {
        // Arrange
        var contact = new Contact
        {
            Street = "123 Main St",
            City = "Sample City",
            State = "Sample State",
            PostalCode = "12345",
            Country = "Sample Country"
        };

        // Act
        var address = contact.Address; // Address is a computed property

        // Assert
        var expected = "123 Main St\nSample City, Sample State 12345\nSample Country";
        Assert.Equal(expected, address);
    }
}