namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpKnowSubSource : BaseDataModel<int>
{
    public string KnowSubSource { get; set; } = null!;

    public int KnowSourceId { get; set; }
    public bool ForSalesOnly { set; get; }
    public bool UseWithWebPortal { get; set; }

    public string? Abbrev { get; set; }

    public virtual LookUpKnowSource KnowSource { get; set; } = null!;
}