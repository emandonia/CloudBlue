namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpGracePeriod : BaseDataModel<int>
{
    public int DisplayOrder { get; set; }

    public string GracePeriod { get; set; } = null!;

    public int GracePeriodHours { get; set; }
}