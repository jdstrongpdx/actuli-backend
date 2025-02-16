using Actuli.Api.Models;
using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class AppUser(string id)
{
    [JsonProperty("id")] public string Id { get; set; } = id;
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("profile")]
    public Profile Profile { get; set; } = new();
    
    [JsonProperty("overview")]
    public Overview Overview { get; set; } = new();
    
    [JsonProperty("goals")]
    public List<Goal> Goals { get; set; } = new List<Goal>();
    
    [JsonProperty("accomplishments")]
    public List<Accomplishment> Accomplishments { get; set; } = new List<Accomplishment>();
    
    [JsonProperty("createdAt")]
    public DateTime? CreatedAt { get; } = DateTime.UtcNow;
    
    [JsonProperty("modifiedAt")]
    public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
    
    public void MarkAsModified()
    {
        ModifiedAt = DateTime.UtcNow;
    }

}