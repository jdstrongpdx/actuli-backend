namespace Actuli.Api.Models.ProfileTypes;

public class Identity
{
    public string Gender { get; set; } // IDENTITY_GENDER_LIST
    public string Sexuality { get; set; } // IDENTITY_SEXUALITY_LIST
    public string RelationshipStatus { get; set; } // IDENTITY_RELATIONSHIP_STATUS_LIST
    public string Nationality { get; set; } // IDENTITY_NATIONALITY_SELECT
    public string CoreValues { get; set; } // IDENTITY_CORE_VALUES_SELECT
    public string TechnologicalLiteracy { get; set; } // SEVEN_LEVEL_LIST
    public string PoliticalValues { get; set; } // IDENTITY_POLITICAL_VALUES_SELECT
}