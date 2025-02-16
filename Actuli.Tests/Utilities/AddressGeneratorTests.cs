using Actuli.Api.Utilities;
using Xunit;

namespace Actuli.Tests.Utilities;

public class AddressGeneratorTests
{
    [Fact]
    public void GenerateAddress_ShouldReturnFormattedAddress_WhenAllFieldsAreValid()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = "Apt 4B";
        string city = "Springfield";
        string state = "IL";
        string postalCode = "62704";
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        var expectedAddress = "123 Main St\nApt 4B\nSpringfield, IL 62704\nUSA";
        Assert.Equal(expectedAddress, result);
    }

    [Fact]
    public void GenerateAddress_ShouldNotIncludeAddress2_WhenAddress2IsNullOrEmpty()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = null; // Address2 is null
        string city = "Springfield";
        string state = "IL";
        string postalCode = "62704";
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        var expectedAddress = "123 Main St\nSpringfield, IL 62704\nUSA";
        Assert.Equal(expectedAddress, result);
    }

    [Fact]
    public void GenerateAddress_ShouldNotIncludeCountry_WhenCountryIsNullOrEmpty()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = "Apt 4B";
        string city = "Springfield";
        string state = "IL";
        string postalCode = "62704";
        string country = ""; // Country is empty

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        var expectedAddress = "123 Main St\nApt 4B\nSpringfield, IL 62704";
        Assert.Equal(expectedAddress, result);
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenMandatoryFieldsAreMissing()
    {
        // Arrange
        string address1 = ""; // Address1 is empty
        string address2 = "Apt 4B";
        string city = "Springfield";
        string state = "IL";
        string postalCode = "62704";
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenCityIsNullOrEmpty()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = "Apt 4B";
        string city = ""; // City is empty
        string state = "IL";
        string postalCode = "62704";
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenStateIsNullOrEmpty()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = "Apt 4B";
        string city = "Springfield";
        string state = ""; // State is empty
        string postalCode = "62704";
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GenerateAddress_ShouldReturnEmptyString_WhenPostalCodeIsNullOrEmpty()
    {
        // Arrange
        string address1 = "123 Main St";
        string address2 = "Apt 4B";
        string city = "Springfield";
        string state = "IL";
        string postalCode = ""; // PostalCode is empty
        string country = "USA";

        // Act
        var result = AddressGenerator.GenerateAddress(address1, address2, city, state, postalCode, country);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}
