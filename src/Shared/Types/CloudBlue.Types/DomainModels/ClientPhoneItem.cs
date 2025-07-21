using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;

public class ClientPhoneItem
{
    public long Id { get; set; }
    public string? PhoneCountryCode { get; set; }
    public string? CountryName { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Invalid country")]
    public int CountryId { get; set; } = 73;


    [RegularExpression(@"^-?\d+$", ErrorMessage = "Area code is invalid")]

    public string? PhoneAreaCode { get; set; }


    [RegularExpression(@"^-?\d+$", ErrorMessage = "Phone is invalid")]

    public string? Phone { get; set; }
    public string DeviceInfo { get; set; } = null!;
    public string? LoweredDeviceInfo { get; set; }
    public string? DeviceType { get; set; }
    public int DeviceTypeId { get; set; } = 1;
    public bool IsDefault { get; set; }



}

