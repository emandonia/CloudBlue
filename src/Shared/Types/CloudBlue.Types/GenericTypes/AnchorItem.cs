namespace CloudBlue.Domain.GenericTypes;

public class AnchorItem
{
    public string PropertyName { set; get; } = null!;
    public string AnchorText { set; get; } = null!;
    public bool NeedsExtraMapping { set; get; }
}