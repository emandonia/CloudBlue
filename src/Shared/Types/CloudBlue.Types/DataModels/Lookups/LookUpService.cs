namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpService : BaseDataModel<int>
{
    public string Service { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public string? Note { get; set; }
}