namespace Actuli.Api.Utilities;

public static class AddressGenerator
{
    public static string GenerateAddress(string street, string city, string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street) ||
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(state) ||
            string.IsNullOrWhiteSpace(postalCode))
        {
            return string.Empty;
        }

        var address = $"{street}\n{city}, {state} {postalCode}";

        if (!string.IsNullOrWhiteSpace(country))
        {
            address += $"\n{country}";
        }

        return address;
    }
}