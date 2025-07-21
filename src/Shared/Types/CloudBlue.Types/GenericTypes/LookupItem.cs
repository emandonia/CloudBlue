namespace CloudBlue.Domain.GenericTypes;

public class LookupItem<T>(string itemName, T itemId, string? extraId, int parentId)
{
    public T ItemId { get; set; } = itemId;
    public int ParentItemId { get; set; } = parentId;
    public string ItemName { get; set; } = itemName;
    public string? ExtraId { get; set; } = extraId;
    public LookupItem<T>[] SubLookUps { set; get; } = [];
}