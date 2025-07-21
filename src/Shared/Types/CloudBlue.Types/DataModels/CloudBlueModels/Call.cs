using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class Call
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public int BranchId { get; set; }

    public string? CallNote { get; set; }

    public int CallTypeId { get; set; }

    public int CallStatusId { get; set; }

    public DateTime CreationDate { get; set; }

    public long CreationDateNumeric { get; set; }

    public long CreatorId { get; set; }

    public string? CallTypeOther { get; set; }

    public int CompanyId { get; set; }

    public int SourceId { get; set; }

    public string? SourceExtra { get; set; }

    public int KnowSourceId { get; set; }

    public string? KnowSourceExtra { get; set; }

    public DateTime? KnowDate { get; set; }

    public long KnowDateNumeric { get; set; }

    public string? StatusReason { get; set; }

    public long HandledBy { get; set; }

    public long CanceledBy { get; set; }

    public long HandledDateNumeric { get; set; }

    public long CanceledDateNumeric { get; set; }

    public DateTime? HandledDate { get; set; }

    public DateTime? CanceledDate { get; set; }

    public int KnowSourceExtraId { get; set; }

    public long LastEventId { get; set; }

    public bool IsArchived { get; set; }

    public int VoidReasonId { get; set; }

    public long ExpRegId { get; set; }

    public long CampaignOwnerId { get; set; }

    public long WebLeadId { get; set; }

    public long CollectiveCampaignId { get; set; }

    public long ProjectCampaignId { get; set; }

    public int ClientCategoryId { get; set; }
}
