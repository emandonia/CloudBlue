namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpResaleTcrStatus : BaseDataModel<int>
{
    public string ResaleTcrStatusName { get; set; } = null!;
}