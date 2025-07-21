namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpCallType : BaseDataModel<int>
{
    public string CallType { get; set; } = null!;
    public int DisplayOrder { get; set; }
}