using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpDistrict
{
    public int Id { get; set; }

    public string DistrictName { get; set; } = null!;

    public int CityId { get; set; }

    public string? DistrictNameAra { get; set; }

    public virtual LookUpCity City { get; set; } = null!;

    public virtual ICollection<LookUpNeighborhood> LookUpNeighborhoods { get; set; } = new List<LookUpNeighborhood>();
}
