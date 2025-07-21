using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class ClientContactDevice
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public int DeviceTypeId { get; set; }

    public string? DeviceInfo { get; set; }

    public string? PhoneCountryCode { get; set; }

    public string? PhoneAreaCode { get; set; }

    public string? Phone { get; set; }

    public bool IsDefault { get; set; }

    public long WebLeadId { get; set; }

    public virtual Client Client { get; set; } = null!;
}
