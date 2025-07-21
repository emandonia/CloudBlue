namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpRotationStatus : BaseDataModel<int>
{
    public bool IsRunning { get; set; }
}