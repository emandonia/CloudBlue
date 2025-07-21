namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpWorkField : BaseDataModel<int>
{
    public string WorkField { get; set; } = null!;
}