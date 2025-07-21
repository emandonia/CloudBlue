namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpVoidReason : BaseDataModel<int>
{
    public string VoidReason { get; set; } = null!;
}