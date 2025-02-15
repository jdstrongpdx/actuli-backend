namespace Actuli.Api.Models.ProfileTypes;

public class Work
{
    public string WorkTitle { get; set; }
    public string Employer { get; set; }
    public string Industry { get; set; } // WORK_INDUSTRY_LIST
    public string CareerLevel { get; set; } // WORK_CAREER_LEVEL_LIST
    public string Wage { get; set; }
    public string WageScale { get; set; } // WORK_WAGE_SCALE_LIST
    public string City { get; set; }
    public string State { get; set; } // STATES_LIST
    public string Country { get; set; } // COUNTRY_LIST
    public string Location { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } // WORK_STATUS_LIST
    public string Description { get; set; }
}