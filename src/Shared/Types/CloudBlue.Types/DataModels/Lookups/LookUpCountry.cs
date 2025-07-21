namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpCountry : BaseDataModel<int>
{
    public string Country { get; set; } = null!;
    public string PhoneCode { get; set; } = null!;
    public virtual ICollection<LookUpCity> LookUpCities { get; set; } = new List<LookUpCity>();
    public int DisplayOrder { get; set; }
}