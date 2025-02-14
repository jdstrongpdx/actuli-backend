using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Actuli.Api.Utilities;
using Newtonsoft.Json;

public class Contact
{
    [JsonProperty("email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }
    
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }
    
    [JsonProperty("lastName")]
    public string? LastName { get; set; }
    [JsonProperty("address1")]
    public string? Address1 { get; set; }
    [JsonProperty("address2")]
    public string? Address2 { get; set; }

    [JsonProperty("city")]
    public string? City { get; set; }
    [JsonProperty("state")]
    public string? State { get; set; }
    [JsonProperty("postalCode")]
    public string? PostalCode { get; set; }
    [JsonProperty("country")]
    public string? Country { get; set; }
    [JsonProperty("address")]
    public string? Address { get; set; }
    [JsonProperty("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }
    [JsonProperty("age")]
    public int? Age { get; set; }
    [JsonProperty("homePhone")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? HomePhone { get; set; }
    [JsonProperty("mobilePhone")]
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? MobilePhone { get; set; }
    [JsonProperty("website")]
    [Url(ErrorMessage = "Invalid URL")]
    public string? Website { get; set; }

    public void GenerateAddress()
    {
        if (string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Address1))
        {
            Address = AddressGenerator.GenerateAddress(Address1, Address2, City, State, PostalCode, Country);
        }
    }
    
    public void GenerateAge()
    {
        if (DateOfBirth.HasValue)
        {
            Age = DateTimeUtils.CalculateAge(DateOfBirth.Value);
        }
    }
}