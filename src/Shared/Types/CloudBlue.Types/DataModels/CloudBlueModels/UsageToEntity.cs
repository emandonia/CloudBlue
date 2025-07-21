using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class UsageToEntity
{
    public int Id { get; set; }

    public int UsageId { get; set; }

    public long EntityId { get; set; }

    public int EntityType { get; set; }
}
