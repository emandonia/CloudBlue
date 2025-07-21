namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpPropertyType : BaseDataModel<int>
{
    public string PropertyType { get; set; } = null!;

    public int UsageId { get; set; }

    public string? PropertyTypeArabic { get; set; }

    public virtual LookUpUsage Usage { get; set; } = null!;
}