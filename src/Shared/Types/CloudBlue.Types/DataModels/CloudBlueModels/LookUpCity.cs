using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpCity
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public int CountryId { get; set; }

    public virtual LookUpCountry Country { get; set; } = null!;

    public virtual ICollection<LookUpDistrict> LookUpDistricts { get; set; } = new List<LookUpDistrict>();
}
