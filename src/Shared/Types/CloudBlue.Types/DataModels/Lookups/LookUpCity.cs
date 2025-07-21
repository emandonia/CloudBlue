namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpCity : BaseDataModel<int>
{
    public string City { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual LookUpCountry Country { get; set; } = null!;

    public virtual ICollection<LookUpDistrict> LookUpDistricts { get; set; } = new List<LookUpDistrict>();
}