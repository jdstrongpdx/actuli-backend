using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Overview
{
    [JsonProperty("location")]
    public string? Location { get; set; }
    
    [JsonProperty("education")]
    public CategorySummary? Education { get; set; }
    
    [JsonProperty("work")]
    public CategorySummary? Work { get; set; }
    
    [JsonProperty("relationships")]
    public CategorySummary? Relationships { get; set; }
    
    [JsonProperty("identity")]
    public CategorySummary? Identity { get; set; }
    
    [JsonProperty("religion")]
    public CategorySummary? Religion { get; set; }
    
    [JsonProperty("travel")]
    public CategorySummary? Travel { get; set; }
    
    [JsonProperty("health")]
    public CategorySummary? Health { get; set; }
    
    [JsonProperty("hobbies")]
    public CategorySummary? Hobbies { get; set; }
    
    [JsonProperty("giving")]
    public CategorySummary? Giving { get; set; }
    
    [JsonProperty("finances")]
    public CategorySummary? Finances { get; set; }
    
    [JsonProperty("housing")]
    public CategorySummary? Housing { get; set; }
    
    [JsonProperty("goals")]
    public string? Goals { get; set; }
    
    [JsonProperty("achievements")]
    public string? Achievements { get; set; }
    
    [JsonProperty("summary")]
    public string? Summary { get; set; }
}