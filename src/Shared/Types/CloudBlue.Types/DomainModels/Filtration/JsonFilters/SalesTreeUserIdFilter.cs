namespace CloudBlue.Domain.DomainModels.Filtration.JsonFilters;

public class SalesTreeUserIdFilter
{
    public List<SalesUserTreeItemFilter> SalesUsers { set; get; } = new();
}