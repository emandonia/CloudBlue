using CloudBlue.Domain.DomainModels.DashboardStuff;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IDashboardRepository : IBaseRepository
{
    Task<LeadTicketsCountsItem[]> GetLeadTicketsCountAsync(int agentId, int managerId, int branchId, int companyId);
    Task<PrimeTcrsCountsItem[]> GetPrimeTcrCountAsync(int agentId, int managerId, int branchId, int companyId);
}