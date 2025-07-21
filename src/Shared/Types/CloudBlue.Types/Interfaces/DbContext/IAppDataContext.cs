using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Domain.Interfaces.DbContext;

public interface IAppDataContext
{
    DbSet<SystemEventTemplate> SystemEventTemplates { get; set; }
    DbSet<SystemEvent> SystemEvents { get; set; }
    DbSet<LeadTicketNotificationLog> LeadTicketNotificationLogs { get; set; }
    DbSet<DashboardNotification> DashboardNotifications { get; set; }
    DbSet<DataLog> DataLogs { get; set; }
    DbSet<LeadTicketsCountsItem> LeadTicketsCountsItems { get; set; }
    DbSet<PrimeTcrsCountsItem> PrimeTcrsCountsItems { get; set; }

    Task UpdateEntityLastEventsAsync(EntityTypes entityType, long entityId, int userId = 0, long leadTicketId = 0);
    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task SaveBulkChangesAsync();
    Task<LeadTicketsCountsItem[]> GetLeadTicketsCountAsync(int agentId, int managerId, int branchId, int companyId);
    Task<PrimeTcrsCountsItem[]> GetPrimeTcrCountAsync(int agentId, int managerId, int branchId, int companyId);
}