namespace CloudBlue.Domain.DomainModels;

public class SystemEventItem
{
    public long Id { get; set; }
    public long ClientId { get; set; }

    public string? EventComment { get; set; }

    public DateTime? EventDateTime { get; set; }

    public DateTime? EventCreationDateTime { get; set; }

    public long EventCreationDateTimeNumeric { get; set; }

    public string? FullName { get; set; }

    public string? EventType { get; set; }

    public string? EventProcess { get; set; }

    public string? ClientName { get; set; }
    public bool Dismissed { get; set; }
    public int EventTypeId { get; set; }
    public int ContactingTypeId { get; set; }
    public int EventProcessId { get; set; }
    public int UserCompanyId { get; set; }
    public int UserBranchId { get; set; }

    public string? ContactingType { get; set; }
    public string? UserPositionName { get; set; }
    public string? UserImagePath { get; set; }
    public int UserId { get; set; }
}