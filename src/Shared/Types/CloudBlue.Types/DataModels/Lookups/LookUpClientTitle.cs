namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpClientTitle : BaseDataModel<int>
{
    public string ClientTitle { get; set; } = null!;
}