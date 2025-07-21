using CloudBlue.Domain.Enums;
using System.Text.Json;

namespace CloudBlue.Domain.DataModels.Crm;

public class Call : BaseDataModel<long>
{
    public long ClientId { get; set; }

    public int BranchId { get; set; }

    public string? CallNote { get; set; }

    public CallTypes CallType { get; set; }

    public CallStatuses CallStatus { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreationDateNumeric { get; set; }

    public int CreatedById { get; set; }

    public string? DurationStr { get; set; }

    public int DurationInSeconds { get; set; }

    public string? CallTypeOther { get; set; }

    public int CompanyId { get; set; }

    public int LeadSourceId { get; set; }

    public string? SourceExtra { get; set; }

    public int KnowSourceId { get; set; }

    public string? KnowSourceExtra { get; set; }

    public DateTime? KnowDate { get; set; }

    public string? StatusReason { get; set; }

    public int HandledById { get; set; }

    public int CanceledById { get; set; }

    public int HandledDateNumeric { get; set; }

    public int CanceledDateNumeric { get; set; }

    public DateTime? HandledDate { get; set; }

    public DateTime? CanceledDate { get; set; }

    public int KnowSourceExtraId { get; set; }

    public long LastEventId { get; set; }

    public bool IsArchived { get; set; }

    public int VoidReasonId { get; set; }

    public long ExpRegId { get; set; }

    public int CampaignOwnerId { get; set; }

    public long WebLeadId { get; set; }

    public int CollectiveCampaignId { get; set; }

    public int ProjectCampaignId { get; set; }

    public JsonDocument? RecentEventsJsonb { get; set; }

    public string? CreatedBy { get; set; }

    public string? HandledBy { get; set; }

    public string? CanceledBy { get; set; }

    public string? Location { get; set; }
}