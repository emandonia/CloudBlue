namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public interface IEntityFilters
{
    string? EntityIds { set; get; }
    DateTime? EntityCreationDateFrom { set; get; }
    DateTime? EntityCreationDateTo { set; get; }
    int CreatedById { set; get; }
    int ModeId { set; get; }
    int CompanyId { set; get; }
    int BranchId { set; get; }
    IEnumerable<int> EntityStatusIds { set; get; }
}