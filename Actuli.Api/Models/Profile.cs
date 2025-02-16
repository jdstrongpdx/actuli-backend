using Actuli.Api.Models.ProfileTypes;
using Newtonsoft.Json;

namespace Actuli.Api.Models;

public class Profile
{
    [JsonProperty("contact")] public Contact? Contact { get; set; } = new Contact();
    
    [JsonProperty("educationList")]
    public List<Education>? EducationList { get; set; } = new List<Education>();
    
    [JsonProperty("workList")]
    public List<Work>? WorkList { get; set; } = new List<Work>();
    
    [JsonProperty("relationshipList")]
    public List<Relationship>? RelationshipsList { get; set; } = new List<Relationship>();
    
    [JsonProperty("identity")]
    public Identity? Identity { get; set; } = new Identity();
    
    [JsonProperty("religionsList")]
    public List<Religion>? ReligionsList { get; set; } = new List<Religion>();
    
    [JsonProperty("travelsList")]
    public List<Travel>? TravelsList { get; set; } = new List<Travel>();

    [JsonProperty("health")] public Health? Health { get; set; } = new Health();
    
    [JsonProperty("activitiesList")]
    public List<Activity>? ActivitiesList { get; set; } = new List<Activity>();
    
    [JsonProperty("givingList")]
    public List<Giving>? GivingList { get; set; } = new List<Giving>();

    [JsonProperty("finances")] public Finances? Finances { get; set; } = new Finances();
}