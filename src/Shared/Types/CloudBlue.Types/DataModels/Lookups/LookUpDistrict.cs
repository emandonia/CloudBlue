namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpDistrict : BaseDataModel<int>
{
    public string District { get; set; } = null!;

    public int CityId { get; set; }

    public string? DistrictArabic { get; set; }

    public virtual LookUpCity City { get; set; } = null!;

    public virtual ICollection<LookUpNeighborhood> LookUpNeighborhoods { get; set; } = new List<LookUpNeighborhood>();
}