using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class AppUser
{
    public AppUser(string id)
    {
        Id = id;
    }
    
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("profile")]
    public Profile Profile { get; set; } = new();
    
    [JsonProperty("overview")]
    public Overview Overview { get; set; } = new();
    
    [JsonProperty("goals")]
    public List<Goal> Goals { get; set; } = new();
    
    [JsonProperty("accomplishments")]
    public List<Accomplishment> Accomplishments { get; set; } = new();
    
    [JsonProperty("createdAt")]
    public DateTime? CreatedAt { get; set; }
    
    [JsonProperty("modifiedAt")]
    public DateTime? ModifiedAt { get; set; }
    
    public void MarkAsCreated()
    {
        CreatedAt = DateTime.UtcNow;
    }
    public void MarkAsModified()
    {
        ModifiedAt = DateTime.UtcNow;
    }

}