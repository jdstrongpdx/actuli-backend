using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class CategorySummary
{
    [JsonProperty("satisfaction")]
    public string Satisfaction { get; set; } // source: SATISFACTION_LIST
    
    [JsonProperty("importance")]
    public string Importance { get; set; }   // source: IMPORTANCE_LIST
    
    [JsonProperty("changeGoalDescription")]
    public string ChangeGoalDescription { get; set; }
    
    [JsonProperty("profileSummary")]
    public string ProfileSummary { get; set; }
    
    [JsonProperty("goalsSummary")]
    public string GoalsSummary { get; set; }
    
    [JsonProperty("achievementsSummary")]
    public string AchievementsSummary { get; set; }
}