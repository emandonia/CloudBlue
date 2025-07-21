namespace CloudBlue.Domain.DomainModels;

public class EntityUsageItem
{
    public long EntityId { get; set; }
    public int EntityTypeId { get; set; }
    public int UsageId { get; set; }
    public string Usage { get; set; } = null!;
}