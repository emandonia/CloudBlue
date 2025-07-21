using CloudBlue.Domain.DomainModels.DashboardStuff;

namespace CloudBlue.Domain.Interfaces.Services;
public interface IDashboardService : IBaseService

{
    Task<DashboardContent> GetDashboardContentsAsync();
}
