namespace CloudBlue.Domain.GenericTypes;
public class LocationItem
{
    public int CountryId { set; get; }
    public string? Country { set; get; }
    public int CityId { set; get; }
    public string? City { set; get; }
    public int DistrictId { set; get; }
    public string? District { set; get; }
    public int NeighborhoodId { set; get; }
    public string? Neighborhood { set; get; }
    public int NeighborhoodTypeId { get; set; } = 2;
}

