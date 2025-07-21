namespace CloudBlue.Domain.GenericTypes;

public class ListResult<T>
{
    public int CurrentPageIndex { get; set; }
    public int CurrentPageSize { get; set; }
    public T[] Items { set; get; } = null!;
    public int TotalCount { set; get; }
}