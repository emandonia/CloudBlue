using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels.Crm;

public class ClientContactDevice
{
    public DeviceTypes DeviceType { get; set; }

    public virtual Client Client { get; set; } = null!;
    public long Id { get; set; }

    public long ClientId { get; set; }

    public string DeviceInfo { get; set; } = null!;

    public string? PhoneCountryCode { get; set; }

    public string? PhoneAreaCode { get; set; }

    public string? Phone { get; set; }

    public long WebLeadId { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedById { get; set; }

    public int CountryId { get; set; }
}