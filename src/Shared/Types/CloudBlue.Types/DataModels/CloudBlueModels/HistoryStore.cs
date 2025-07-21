using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class HistoryStore
{
    public DateTime Timemark { get; set; }

    public string TableName { get; set; } = null!;

    public string PkDateSrc { get; set; } = null!;

    public string PkDateDest { get; set; } = null!;

    public short RecordState { get; set; }
}
