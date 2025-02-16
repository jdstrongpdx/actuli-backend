using System.ComponentModel.DataAnnotations;
using Actuli.Api.Models.ProfileTypes;

namespace Actuli.Tests.Models.ProfileTypes;

public class ContactTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultNullValues()
    {
        var contact = new Contact();

        Assert.Null(contact.Email);
        Assert.Null(contact.FirstName);
        Assert.Null(contact.LastName);
        Assert.Null(contact.Address1);
        Assert.Null(contact.Address2);
        Assert.Null(contact.City);
        Assert.Null(contact.State);
        Assert.Null(contact.PostalCode);
        Assert.Null(contact.Country);
        Assert.Null(contact.Address);
        Assert.Null(contact.DateOfBirth);
        Assert.Null(contact.Age);
        Assert.Null(contact.HomePhone);
        Assert.Null(contact.MobilePhone);
        Assert.Null(contact.Website);
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("not-an-email", false)]
    [InlineData(null, true)]
    public void Email_ShouldValidateCorrectly(string email, bool isValid)
    {
        var contact = new Contact { Email = email };

        var context = new ValidationContext(contact);
        var results = new System.Collections.Generic.List<ValidationResult>();

        var isValidEmail = Validator.TryValidateObject(contact, context, results, true);

        Assert.Equal(isValid, isValidEmail);
    }

    [Theory]
    [InlineData("123-456-7890", true)]
    [InlineData("not-a-phone", false)]
    public void HomePhone_ShouldValidateCorrectly(string homePhone, bool isValid)
    {
        var contact = new Contact { HomePhone = homePhone };

        var context = new ValidationContext(contact);
        var results = new System.Collections.Generic.List<ValidationResult>();

        var isValidPhone = Validator.TryValidateObject(contact, context, results, true);

        Assert.Equal(isValid, isValidPhone);
    }

    [Theory]
    [InlineData("http://example.com", true)]
    [InlineData("not-a-url", false)]
    public void Website_ShouldValidateCorrectly(string website, bool isValid)
    {
        var contact = new Contact { Website = website };

        var context = new ValidationContext(contact);
        var results = new System.Collections.Generic.List<ValidationResult>();

        var isValidUrl = Validator.TryValidateObject(contact, context, results, true);

        Assert.Equal(isValid, isValidUrl);
    }

    [Fact]
    public void GenerateAddress_ShouldConstructAddress_WhenAddress1IsNotNull()
    {
        var contact = new Contact
        {
            Address1 = "123 Main St",
            Address2 = "Apt 4B",
            City = "New York",
            State = "NY",
            PostalCode = "10001",
            Country = "USA"
        };

        contact.GenerateAddress();

        Assert.NotNull(contact.Address);
    }

    [Fact]
    public void GenerateAddress_ShouldNotChangeAddress_IfAlreadySet()
    {
        var contact = new Contact
        {
            Address = "Already Set Address",
            Address1 = "123 Main St",
            City = "New York"
        };

        contact.GenerateAddress();

        Assert.Equal("Already Set Address", contact.Address);
    }

    [Fact]
    public void GenerateAge_ShouldCalculateAge_WhenDateOfBirthIsProvided()
    {
        var dateOfBirth = new DateTime(1990, 1, 1);
        var expectedAge = DateTime.Now.Year - dateOfBirth.Year;

        var contact = new Contact
        {
            DateOfBirth = dateOfBirth
        };

        contact.GenerateAge();

        Assert.Equal(expectedAge, contact.Age);
    }

    [Fact]
    public void GenerateAge_ShouldNotChangeAge_WhenDateOfBirthIsNull()
    {
        var contact = new Contact();

        contact.GenerateAge();

        Assert.Null(contact.Age);
    }
}
