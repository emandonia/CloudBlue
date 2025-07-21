using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;

namespace CloudBlue.Data.Repositories.App;
public class DashboardRepository(IAppDataContext dbContext) : IDashboardRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }
    public Task<LeadTicketsCountsItem[]> GetLeadTicketsCountAsync(int agentId, int managerId, int branchId, int companyId)
    {
        return dbContext.GetLeadTicketsCountAsync(agentId, managerId, branchId, companyId);
    }

    public Task<PrimeTcrsCountsItem[]> GetPrimeTcrCountAsync(int agentId, int managerId, int branchId, int companyId)
    {
        return dbContext.GetPrimeTcrCountAsync(agentId, managerId, branchId, companyId);

    }
}
