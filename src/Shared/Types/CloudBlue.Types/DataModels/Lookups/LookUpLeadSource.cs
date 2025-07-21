namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpLeadSource : BaseDataModel<int>
{
    public bool ForSalesOnly { set; get; }
    public string LeadSource { get; set; } = null!;
}