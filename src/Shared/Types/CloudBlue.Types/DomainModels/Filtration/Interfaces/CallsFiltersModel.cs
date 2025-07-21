namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class CallsFiltersModel : SearchPager, IClientFilters, IEntityFilters, ILeadSourceFilters
{
    public string? ClientName { get; set; }
    public string? ClientNameArabic { get; set; }

    // "(0020) 012 2121402"
    public string? ClientContactDevice { get; set; }

    public int ClientCategoryId { get; set; }
    public int InternationalOnly { get; set; }
    public string? EntityIds { get; set; }
    public DateTime? EntityCreationDateFrom { get; set; }
    public DateTime? EntityCreationDateTo { get; set; }
    public int CreatedById { get; set; }
    public int ModeId { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public IEnumerable<int> EntityStatusIds { get; set; } = new List<int>();
    public int LeadSourceId { get; set; }
    public int KnowSourceId { get; set; }
    public int KnowSubSourceId { get; set; }
    public int CallTypeId { get; set; }
}