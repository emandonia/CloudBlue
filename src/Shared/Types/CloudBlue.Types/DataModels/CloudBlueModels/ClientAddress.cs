using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class ClientAddress
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public int AddressTypeId { get; set; }

    public int CountryId { get; set; }

    public int CityId { get; set; }

    public int DistrictId { get; set; }

    public string? AddressInfo { get; set; }

    public string? AddressAr { get; set; }

    public virtual Client Client { get; set; } = null!;
}
