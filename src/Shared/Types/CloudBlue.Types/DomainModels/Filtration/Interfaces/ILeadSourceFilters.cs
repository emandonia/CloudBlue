namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public interface ILeadSourceFilters
{
    int LeadSourceId { set; get; }

    int KnowSourceId { set; get; }
    int KnowSubSourceId { set; get; }
}