using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpKnowSubSource
{
    public int Id { get; set; }

    public string KnowSubSource { get; set; } = null!;

    public int KnowSourceId { get; set; }

    public bool UseWithWebPortal { get; set; }

    public string? Abbrev { get; set; }

    public virtual LookUpKnowSource KnowSource { get; set; } = null!;
}
