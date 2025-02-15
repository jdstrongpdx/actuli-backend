using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Accomplishment
{
    [JsonProperty("accomplishmentId")]
    public string Id { get; set; } 
    
    [JsonProperty("goalId")]
    public string GoalId { get; set; } 
    
    [JsonProperty("completedAt")]
    public DateTime completedAt { get; set; } 
    
    [JsonProperty("description")]
    public string Description { get; set; } 

    [JsonProperty("notes")]
    public string Notes { get; set; } 
}

