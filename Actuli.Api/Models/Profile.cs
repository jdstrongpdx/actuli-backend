using Actuli.Api.Models.ProfileTypes;
using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Profile
{
    [JsonProperty("contact")]
    public Contact? Contact { get; set; }
    
    [JsonProperty("educationList")]
    public List<Education>? EducationList { get; set; }
    
    [JsonProperty("workList")]
    public List<Work>? WorkList { get; set; }
    
    [JsonProperty("relationshipList")]
    public List<Relationship>? RelationshipsList { get; set; }
    
    [JsonProperty("identity")]
    public Identity? Identity { get; set; }
    
    [JsonProperty("religionsList")]
    public List<Religion>? ReligionsList { get; set; }
    
    [JsonProperty("travelsList")]
    public List<Travel>? TravelsList { get; set; }
    
    [JsonProperty("health")]
    public Health? Health { get; set; }
    
    [JsonProperty("activitiesList")]
    public List<Activity>? ActivitiesList { get; set; }
    
    [JsonProperty("givingList")]
    public List<Giving>? GivingList { get; set; }
    
    [JsonProperty("finances")]
    public Finances? Finances { get; set; }
}