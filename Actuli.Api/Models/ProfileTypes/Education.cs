namespace Actuli.Api.Models.ProfileTypes;

public class Education
{
    public required string School { get; set; }
    public required string DegreeType { get; set; } // EDUCTION_DEGREES_LIST
    public required string DegreeName { get; set; }
    public string? City { get; set; }
    public string? State { get; set; } // STATES_LIST
    public string? Country { get; set; } // COUNTRY_LIST
    public string Status { get; set; } // EDUCATION_STATUS_LIST
    public DateTime? CompletionDate { get; set; }
    public string? Grade { get; set; }
    public string? GradeScale { get; set; } // EDUCATION_GRADE_SCALE_LIST
    public string? Description { get; set; }
    public string? PersonalImportance { get; set; }
    public string? CareerImportance { get; set; }
}
