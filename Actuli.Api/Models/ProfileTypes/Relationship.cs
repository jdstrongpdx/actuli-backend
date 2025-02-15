namespace Actuli.Api.Models.ProfileTypes;

public class Relationship
{
    public string Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string RelationshipType { get; set; } // RELATIONSHIP_TYPE_LIST
    public string Wage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string InteractionFrequency { get; set; } // FREQUENCY_LIST
    public string Status { get; set; } // RELATIONSHIP_STATUS_LIST
    public string RelationshipImportance { get; set; } // IMPORTANCE_LIST
    public string Description { get; set; }
}