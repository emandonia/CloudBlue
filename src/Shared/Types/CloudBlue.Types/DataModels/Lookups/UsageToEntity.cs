using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class UsageToEntity : BaseDataModel<int>
{
    public int UsageId { set; get; }
    public LookUpUsage Usage { set; get; } = null!;
    public long EntityId { set; get; }

    public EntityTypes EntityType { set; get; }
}