namespace CloudBlue.Domain.DomainModels.Filtration.JsonFilters;

public class AgentFeedbackFilter(int typeId)
{
    public int ContactingTypeId { get; set; } = typeId;
}