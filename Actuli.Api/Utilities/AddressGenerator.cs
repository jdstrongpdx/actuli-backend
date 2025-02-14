namespace Actuli.Api.Utilities;

public static class AddressGenerator
{
    public static string GenerateAddress(string address1, string? address2, string city, string state,
        string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(address1) ||
            string.IsNullOrWhiteSpace(city) ||
            string.IsNullOrWhiteSpace(state) ||
            string.IsNullOrWhiteSpace(postalCode))
        {
            return string.Empty;
        }

        // Start with address1
        var address = address1;

        // Add address2 if present
        if (!string.IsNullOrWhiteSpace(address2))
        {
            address += $"\n{address2}";
        }

        // Add city, state, and postal code
        address += $"\n{city}, {state} {postalCode}";

        // Add country if present
        if (!string.IsNullOrWhiteSpace(country))
        {
            address += $"\n{country}";
        }

        return address;
    }
}