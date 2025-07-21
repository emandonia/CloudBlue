namespace CloudBlue.Domain.DomainModels.Filtration.JsonFilters;

public class ClientDeviceInfoFilter(string deviceInfo)
{
    public string LoweredDeviceInfo { get; set; } = deviceInfo;
}