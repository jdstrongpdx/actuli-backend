namespace Actuli.Api.Utilities;

public static class DateTimeUtils
{
    public static int CalculateAge(DateTime birthDate, DateTime? currentDate = null)
    {
        // Use the current date if no date is passed in
        DateTime effectiveDate = currentDate ?? DateTime.Today;

        // Calculate the preliminary age
        int age = effectiveDate.Year - birthDate.Year;

        // If the person's birthday hasn't occurred yet this year, subtract 1
        if (effectiveDate < birthDate.AddYears(age))
        {
            age--;
        }

        // Handle negative ages (where the birth date is in the future)
        return age < 0 ? 0 : age;
    }
}