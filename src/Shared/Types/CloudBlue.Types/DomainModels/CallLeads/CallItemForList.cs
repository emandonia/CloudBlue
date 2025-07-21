using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class CallItemForList : IEntityBaseItem
{
    public string? ClientName { get; set; }
    public string? Location { get; set; }

    public string? CallTypeFontColor { get; set; }

    public string? CallTypeBackgroundColor { get; set; }

    public string? CallStatusFontColor { get; set; }

    public string? CallStatusBackgroundColor { get; set; }

    public long ClientId { get; set; }

    public string? CallNote { get; set; }

    public CallTypes CallTypeId { get; set; }

    public CallStatuses CallStatusId { get; set; }

    public string? DurationStr { get; set; }

    public int DurationInSeconds { get; set; }

    public string? StatusReason { get; set; }

    public DateTime? HandledDate { get; set; }

    public DateTime? CanceledDate { get; set; }

    public string? SalesType { get; set; }
    public string? Usage { get; set; }

    public bool? IsArchived { get; set; }

    public string? HandledBy { get; set; }

    public string? CanceledBy { get; set; }

    public string? CallStatus { get; set; }

    public string? CallType { get; set; }

    public string? LeadSource { get; set; }

    public string? KnowSource { get; set; }

    public string? KnowSubSource { get; set; }

    public DateTime? ClientBirthDate { get; set; }

    public bool? IsVip { get; set; }
    public bool? IsPotential { get; set; }
    public ClientPhoneItem[] ClientContactDevices { get; set; } = [];
    public SystemEventItem[] SystemEvents { get; set; } = [];
    public string? ClientTitle { get; set; }
    public string? ClientType { get; set; }
    public string? PropertyType { get; set; }
    public string? ClientNameArabic { get; set; }
    public string? ClientPhone { get; set; }
    public object? CallTypeBadgeStyle { get; set; }
    public object? CallStatusBadgeStyle { get; set; }
    public string? ClientCategory { get; set; }
    public string? Gender { get; set; }
    public string? WorkField { get; set; }
    public string? ClientOccupation { get; set; }
    public string? ClientCompanyName { get; set; }
    public long Id { get; set; }
    public SystemPrivileges[] AllowedActions { get; set; } = [];
    public string? BranchName { get; set; }

    public DateTime CreationDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? CompanyName { get; set; }
}