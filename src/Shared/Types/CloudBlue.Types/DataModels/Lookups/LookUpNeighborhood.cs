namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpNeighborhood : BaseDataModel<int>
{
    public string Neighborhood { get; set; } = null!;

    public int DistrictId { get; set; }

    public int TypeId { get; set; }

    public string? WebSiteName { get; set; }

    public string? NeighborhoodNameAra { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual LookUpDistrict District { get; set; } = null!;
}