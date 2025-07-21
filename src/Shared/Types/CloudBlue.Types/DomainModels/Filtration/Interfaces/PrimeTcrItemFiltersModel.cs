namespace CloudBlue.Domain.DomainModels.Filtration.Interfaces;

public class PrimeTcrItemFiltersModel
{
    public bool UseTcrCompany { get; set; }
    public bool UserAgentOr { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public long PrimeTcrId { get; set; }

    public int AgentId { get; set; }
    public bool AgentsRecursive { get; set; }
    public List<int> ManagersIds { get; set; } = new();
}