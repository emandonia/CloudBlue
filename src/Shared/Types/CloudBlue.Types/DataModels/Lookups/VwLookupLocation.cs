namespace CloudBlue.Domain.DataModels.Lookups;

public class VwLookupLocation
{
    public int CountryId { get; set; }

    public string? Country { get; set; }

    public int CityId { get; set; }

    public string? City { get; set; }

    public int DistrictId { get; set; }

    public string? District { get; set; }

    public int Id { get; set; }
    public int NeighborhoodId { get; set; }

    public string? Neighborhood { get; set; }

    public int TypeId { get; set; }
}
