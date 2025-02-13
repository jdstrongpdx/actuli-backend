namespace Actuli.Api.Utilities;

public static class DateTimeUtils
{
    public static int CalculateAge(DateTime birthDate, DateTime? endDate = null)
    {
        // Use the current date if none is provided
        DateTime now = endDate ?? DateTime.Now;

        // Calculate the basic difference in years
        int age = now.Year - birthDate.Year;

        // Check if the birth date hasn't occurred yet in the current year
        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
        {
            age--;
        }

        return age;
    }
}