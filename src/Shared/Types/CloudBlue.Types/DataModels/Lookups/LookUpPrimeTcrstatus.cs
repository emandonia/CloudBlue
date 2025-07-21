namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpPrimeTcrStatus : BaseDataModel<int>
{
    public string PrimeTcrStatus { get; set; } = null!;
}