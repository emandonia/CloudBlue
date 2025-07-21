using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpNeighborhood
{
    public int Id { get; set; }

    public string? NeighborhoodName { get; set; }

    public int DistrictId { get; set; }

    public int TypeId { get; set; }

    public string? WebSiteName { get; set; }

    public string? NeighborhoodNameAra { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public virtual LookUpDistrict District { get; set; } = null!;
}
