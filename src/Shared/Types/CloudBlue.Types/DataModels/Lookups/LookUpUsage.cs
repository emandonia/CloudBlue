namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpUsage : BaseDataModel<int>
{
    public string Usage { get; set; } = null!;
    public string? Note { get; set; }
    public virtual ICollection<LookUpPropertyType> LookUpPropertyTypes { get; set; } = new List<LookUpPropertyType>();
    public virtual ICollection<UsageToEntity> UsageToEntities { get; set; } = new List<UsageToEntity>();
}