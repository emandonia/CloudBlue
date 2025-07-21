namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpDeviceType : BaseDataModel<int>
{
    public string DeviceType { get; set; } = null!;
}