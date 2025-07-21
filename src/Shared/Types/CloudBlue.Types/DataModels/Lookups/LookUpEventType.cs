namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpEventType : BaseDataModel<int>
{
    public string? EventType { get; set; }
}