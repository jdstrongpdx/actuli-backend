using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Actuli.Api.Utilities;

namespace Actuli.Api.Models;

public class AppUser
{
    public AppUser(string id)
    {
        Id = id;
    }
    
    [JsonProperty("id")]
    public string Id { get; set; }  // Derived from oid (user's object id) in Azure AD
    
    [JsonProperty("profile")]
    public Profile Profile { get; set; } = new();
    [JsonProperty("username")]
    public string? Username { get; set; } // Derived from preferred_username in Azure AD
    [JsonProperty("name")]
    public string? Name { get; set; } // Derived from name in Azure AD
    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [JsonProperty("modifiedAt")]
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow; 
}