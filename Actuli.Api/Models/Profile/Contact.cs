using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Actuli.Api.Utilities;

public class Contact
{
    [JsonPropertyName("username")] public string? Username { get; set; }

    [JsonPropertyName("email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [JsonPropertyName("firstName")] public string? FirstName { get; set; }

    [JsonPropertyName("lastName")] public string? LastName { get; set; }

    [JsonPropertyName("street")] public string? Street { get; set; }

    [JsonPropertyName("city")] public string? City { get; set; }

    [JsonPropertyName("state")] public string? State { get; set; }

    [JsonPropertyName("postalCode")] public string? PostalCode { get; set; }

    [JsonPropertyName("country")] public string? Country { get; set; }

    [JsonPropertyName("address")]
    public string Address => AddressGenerator.GenerateAddress(Street, City, State, PostalCode, Country);
    
    [JsonPropertyName("dateOfBirth")] public DateTime? DateOfBirth { get; set; }

    public int? Age => DateOfBirth.HasValue ? DateTimeUtils.CalculateAge(DateOfBirth.Value) : null;
}