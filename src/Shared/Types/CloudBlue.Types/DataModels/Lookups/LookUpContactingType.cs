namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpContactingType : BaseDataModel<int>
{
    public string ContactingType { get; set; } = null!;
    public int DisplayOrder { get; set; }
    public bool CountAsFeedBack { get; set; }
    public bool NeedsDate { get; set; }
    public bool IsEssential { get; set; }
}