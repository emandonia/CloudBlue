namespace CloudBlue.Domain.DataModels;

public class SystemEvent : BaseDataModel<long>
{
    public long ClientId { get; set; }

    public int EventTypeId { get; set; }

    public string? EventComment { get; set; }

    public int UserId { get; set; }

    public int EntityTypeId { get; set; }

    public long EntityId { get; set; }

    public DateTime? EventDateTime { get; set; }

    public long EventDateTimeNumeric { get; set; }

    public int ContactingTypeId { get; set; }

    public int EventProcessId { get; set; }

    public DateTime? EventCreationDateTime { get; set; }

    public long EventCreationDateTimeNumeric { get; set; }

    public bool Dismissed { get; set; }

    public int FromUserId { get; set; }

    public int ToUserId { get; set; }

    public bool? EventSource { get; set; }

    public bool Impersonated { get; set; }

    public int OriginalUserId { get; set; }

    public bool IsConverted { get; set; }
}