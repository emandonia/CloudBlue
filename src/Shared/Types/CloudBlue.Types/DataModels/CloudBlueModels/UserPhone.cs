using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class UserPhone
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int? DeviceTypeId { get; set; }

    public string? DeviceInfo { get; set; }

    public string? PhoneCountryCode { get; set; }

    public string? PhoneAreaCode { get; set; }

    public string? Phone { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime Created { get; set; }
}
