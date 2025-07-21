namespace CloudBlue.Domain.DomainModels.Filtration;

public class SearchPager
{
    public bool ExportMode { get; set; }

    public string SortDirection { get; set; } = "desc";
    public string SortField { get; set; } = "Id";
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 10;
    public string? ExtraFilters { get; set; }
}