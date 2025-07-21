using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpPropertyType
{
    public int Id { get; set; }

    public string? PropertyType { get; set; }

    public int UsageId { get; set; }

    public string? PropertyTypeArabic { get; set; }

    public string? Pfabbrev { get; set; }
}
