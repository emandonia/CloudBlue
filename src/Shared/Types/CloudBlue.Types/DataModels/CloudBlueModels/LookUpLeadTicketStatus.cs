using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpLeadTicketStatus
{
    public int Id { get; set; }

    public int ItemOrder { get; set; }

    public string? LeadTicketStatus { get; set; }
}
