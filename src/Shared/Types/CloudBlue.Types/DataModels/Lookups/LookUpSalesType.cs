namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpSalesType : BaseDataModel<int>
{
    public string SalesType { get; set; } = null!;
}