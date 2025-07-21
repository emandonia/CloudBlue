using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpKnowSource
{
    public int Id { get; set; }

    public string? KnowSourceName { get; set; }

    public virtual ICollection<LookUpKnowSubSource> LookUpKnowSubSources { get; set; } = new List<LookUpKnowSubSource>();
}
