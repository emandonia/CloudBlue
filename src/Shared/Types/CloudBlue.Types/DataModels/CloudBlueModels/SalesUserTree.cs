using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class SalesUserTree
{
    public int Id { get; set; }

    public long UserId { get; set; }

    public string UserFullName { get; set; } = null!;

    public long ParentId { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime Created { get; set; }
}
