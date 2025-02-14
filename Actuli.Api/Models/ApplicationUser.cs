namespace Actuli.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Actuli.Api.Utilities;

public class ApplicationUser (Guid UserId)
{
    [Key] 
    public Guid UserId { get; set; } = UserId; // Derived from oid (user's object id) in Azure AD
    
    [JsonPropertyName("username")] public string? Username { get; set; } // Derived from preferred_username in Azure AD
    
    [JsonPropertyName("name")] public string? Name { get; set; } // Derived from name in Azure AD

    [JsonPropertyName("email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [JsonPropertyName("firstName")] public string? FirstName { get; set; }

    [JsonPropertyName("lastName")] public string? LastName { get; set; }

    [JsonPropertyName("address1")] public string? Address1 { get; set; }
    
    [JsonPropertyName("address2")] public string? Address2 { get; set; }

    [JsonPropertyName("city")] public string? City { get; set; }

    [JsonPropertyName("state")] public string? State { get; set; }

    [JsonPropertyName("postalCode")] public string? PostalCode { get; set; }

    [JsonPropertyName("country")] public string? Country { get; set; }

    [JsonPropertyName("address")] public string? Address { get; set; }
    
    [JsonPropertyName("dateOfBirth")] public DateTime? DateOfBirth { get; set; }
    
    [JsonPropertyName("age")] public int? Age { get; set; }
    
    [JsonPropertyName("homePhone")] 
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? HomePhone { get; set; }
    
    [JsonPropertyName("mobilePhone")] 
    [Phone(ErrorMessage = "Invalid phone number")]
    public string? MobilePhone { get; set; }

    [JsonPropertyName("website")] 
    [Url(ErrorMessage = "Invalid URL")]
    public string? Website { get; set; }
    
    [JsonPropertyName("createdAt")] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("modifiedAt")] 
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow; 

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