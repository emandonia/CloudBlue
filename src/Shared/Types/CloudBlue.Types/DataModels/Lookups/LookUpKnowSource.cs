namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpKnowSource : BaseDataModel<int>
{
    public string KnowSource { get; set; } = null!;
    public bool ForSalesOnly { set; get; }
    public virtual ICollection<LookUpKnowSubSource> KnowSubSources { get; set; } = new List<LookUpKnowSubSource>();
}