using CloudBlue.Domain.Enums;
using System.Text.Json;

namespace CloudBlue.Domain.DataModels.Crm;

public class VwCall
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public int BranchId { get; set; }

    public string? CallNote { get; set; }

    public CallTypes CallTypeId { get; set; }

    public CallStatuses CallStatusId { get; set; }

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

    public int CampaignOwnerId { get; set; }

    public int CollectiveCampaignId { get; set; }

    public int ProjectCampaignId { get; set; }

    public JsonDocument? RecentEventsJsonb { get; set; }

    public string? CreatedBy { get; set; }
    public string? SalesType { get; set; }
    public string? Usage { get; set; }

    public string? HandledBy { get; set; }

    public string? CanceledBy { get; set; }

    public string? BranchName { get; set; }

    public string? CompanyName { get; set; }

    public string? CallStatus { get; set; }

    public string? CallType { get; set; }

    public string? Location { get; set; }
    public string? ClientName { get; set; }
    public string? ClientNameLowered { get; set; }
    public string? ClientNameArabic { get; set; }

    public int ClientCategoryId { get; set; }

    public string? LeadSource { get; set; }

    public string? KnowSource { get; set; }

    public string? KnowSubSource { get; set; }

    public JsonDocument? ContactDevicesJsonb { get; set; }

    public bool IsVip { get; set; }

    public bool IsPotential { get; set; }
    public string? ClientType { get; set; }
    public string? ClientTitle { get; set; }
    public string? PropertyType { get; set; }

    public string? CallTypeFontColor { get; set; }

    public string? CallTypeBackgroundColor { get; set; }

    public string? CallStatusFontColor { get; set; }

    public string? CallStatusBackgroundColor { get; set; }
    public string? ClientCategory { get; set; }
    public DateTime? ClientBirthDate { get; set; }

    public string? Gender { get; set; }
    public string? WorkField { get; set; }
    public string? ClientOccupation { get; set; }
    public string? ClientCompanyName { get; set; }
}