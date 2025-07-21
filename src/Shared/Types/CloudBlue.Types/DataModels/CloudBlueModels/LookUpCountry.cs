using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpCountry
{
    public int Id { get; set; }

    public string CountryName { get; set; } = null!;

    public string? PhoneCode { get; set; }

    public int DisplayOrder { get; set; }

    public virtual ICollection<LookUpCity> LookUpCities { get; set; } = new List<LookUpCity>();
}
