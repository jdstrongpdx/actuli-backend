using Actuli.Api.Utilities;
using Xunit;

namespace Actuli.Tests.Utilities;

public class AddressGeneratorTests
{
    [Fact]
    public void GenerateAddress_ShouldReturnFormattedAddress_WhenAllFieldsAreProvided()
    {
        // Arrange
        var street = "123 Main St";
        var city = "Sample City";
        var state = "Sample State";
        var postalCode = "12345";
        var country = "Sample Country";

        // Act
        var result = AddressGenerator.GenerateAddress(street, city, state, postalCode, country);

        // Assert
        var expected = "123 Main St\nSample City, Sample State 12345\nSample Country";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateAddress_ShouldOmitCountry_WhenCountryIsEmpty()
    {
        // Arrange
        var street = "123 Main St";
        var city = "Sample City";
        var state = "Sample State";
        var postalCode = "12345";
        var country = string.Empty;

        // Act
        var result = AddressGenerator.GenerateAddress(street, city, state, postalCode, country);

        // Assert
        var expected = "123 Main St\nSample City, Sample State 12345";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenAnyRequiredFieldIsMissing()
    {
        // Act & Assert
        Assert.Equal(string.Empty,
            AddressGenerator.GenerateAddress("", "City", "State", "12345", "Country")); // Missing Street
        Assert.Equal(string.Empty,
            AddressGenerator.GenerateAddress("123 Main St", "", "State", "12345", "Country")); // Missing City
        Assert.Equal(string.Empty,
            AddressGenerator.GenerateAddress("123 Main St", "City", "", "12345", "Country")); // Missing State
        Assert.Equal(string.Empty,
            AddressGenerator.GenerateAddress("123 Main St", "City", "State", "", "Country")); // Missing PostalCode
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenFieldsContainOnlyWhitespace()
    {
        // Arrange
        var street = "  ";
        var city = "  ";
        var state = "  ";
        var postalCode = "  ";
        var country = "Sample Country";

        // Act
        var result = AddressGenerator.GenerateAddress(street, city, state, postalCode, country);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateAddress_ShouldHandleNullValuesForOptionalFields()
    {
        // Arrange
        var street = "123 Main St";
        var city = "Sample City";
        var state = "Sample State";
        var postalCode = "12345";
        string? country = null;

        // Act
        var result = AddressGenerator.GenerateAddress(street, city, state, postalCode, country);

        // Assert
        var expected = "123 Main St\nSample City, Sample State 12345";
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GenerateAddress_ShouldNormalizeFormatting_WhenFieldsAreIncomplete()
    {
        // Arrange
        var street = "123 Main St";
        var city = "Sample City";
        var state = "Sample State";
        var postalCode = "12345";
        var country = "   "; // Whitespace country

        // Act
        var result = AddressGenerator.GenerateAddress(street, city, state, postalCode, country);

        // Assert
        var expected = "123 Main St\nSample City, Sample State 12345";
        Assert.Equal(expected, result);
    }
}
