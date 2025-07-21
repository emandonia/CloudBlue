namespace CloudBlue.Domain.DomainModels.Filtration.JsonFilters;

public class ClientCountryInfoFilter(string countryCode)
{
    public string PhoneCountryCode { get; set; } = countryCode;
}