namespace CloudBlue.Domain.DataModels;

public class VwSystemEvent
{
    public long? Id { get; set; }

    public int? ClientId { get; set; }

    public int? EventTypeId { get; set; }

    public string? EventComment { get; set; }

    public int? UserId { get; set; }

    public int? EntityTypeId { get; set; }

    public int? EntityId { get; set; }

    public DateTime? EventDateTime { get; set; }

    public int? EventDateTimeNumeric { get; set; }

    public int? ContactingTypeId { get; set; }

    public int? EventProcessId { get; set; }

    public DateTime? EventCreationDateTime { get; set; }

    public long? EventCreationDateTimeNumeric { get; set; }

    public bool? Dismissed { get; set; }

    public string? FullName { get; set; }

    public string? EventType { get; set; }

    public string? EventProcess { get; set; }

    public bool? Impersonated { get; set; }

    public int? OriginalUserId { get; set; }

    public bool? IsConverted { get; set; }

    public bool? EventSource { get; set; }

    public int? ToUserId { get; set; }

    public int? FromUserId { get; set; }

    public string? ClientName { get; set; }

    public string? ContactingType { get; set; }
}