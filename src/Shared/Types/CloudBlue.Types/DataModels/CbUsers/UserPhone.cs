namespace CloudBlue.Domain.DataModels.CbUsers;

public class UserPhone : BaseDataModel<long>
{
    public int UserId { get; set; }

    public int DeviceTypeId { get; set; }

    public string? DeviceInfo { get; set; }

    public string? PhoneCountryCode { get; set; }

    public string? PhoneAreaCode { get; set; }
    public User User { set; get; } = null!;
    public VwUser VwUser { set; get; } = null!;
    public string? Phone { get; set; }

    public bool IsDefault { get; set; }
}