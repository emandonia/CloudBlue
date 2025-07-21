namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpCallStatus : BaseDataModel<int>
{
    public string CallStatus { get; set; } = null!;
    public int DisplayOrder { set; get; }
}