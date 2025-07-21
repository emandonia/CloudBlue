using CloudBlue.Data.Configurations.App;
using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.DataContext;

#pragma warning disable S3881
public class AppDataContext : DbContext, IAppDataContext
#pragma warning restore S3881
{
    public AppDataContext()
    {
    }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }

    public DbSet<SystemEventTemplate> SystemEventTemplates { get; set; }
    public DbSet<SystemEvent> SystemEvents { get; set; }
    public DbSet<LeadTicketNotificationLog> LeadTicketNotificationLogs { get; set; }
    public DbSet<DashboardNotification> DashboardNotifications { get; set; }
    public DbSet<DataLog> DataLogs { get; set; }
    public DbSet<LeadTicketsCountsItem> LeadTicketsCountsItems { get; set; }
    public DbSet<PrimeTcrsCountsItem> PrimeTcrsCountsItems { get; set; }

    public async Task UpdateEntityLastEventsAsync(EntityTypes entityType, long entityId, int userId = 0,
        long leadTicketId = 0)
    {
        if (entityType == EntityTypes.Call)
        {
            await Database.ExecuteSqlRawAsync("CALL prc_update_call_last_events({0})", entityId);
        }
        else if (entityType == EntityTypes.LeadTicket)
        {
            await Database.ExecuteSqlRawAsync("CALL prc_update_lead_ticket_last_events({0},{1})", entityId, userId);
        }
        else if (entityType == EntityTypes.PrimeTcr)
        {
            await Database.ExecuteSqlRawAsync("CALL prc_update_prime_tcr_last_events({0},{1})", entityId, leadTicketId);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    public async Task SaveBulkChangesAsync()
    {
        await this.BulkSaveChangesAsync();
    }

    public async Task<LeadTicketsCountsItem[]> GetLeadTicketsCountAsync(int agentId, int managerId, int branchId, int companyId)
    {
        return await LeadTicketsCountsItems.FromSqlRaw<LeadTicketsCountsItem>("SELECT * FROM prc_get_lead_counts({0}, {1}, {2}, {3})", agentId, managerId, branchId, companyId).ToArrayAsync();
    }

    public async Task<PrimeTcrsCountsItem[]> GetPrimeTcrCountAsync(int agentId, int managerId, int branchId, int companyId)
    {
        return await PrimeTcrsCountsItems.FromSqlRaw<PrimeTcrsCountsItem>("SELECT * FROM prc_get_prime_tcrs_counts({0}, {1}, {2}, {3})", agentId, managerId, branchId, companyId).ToArrayAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SystemEventConfiguration());
        modelBuilder.ApplyConfiguration(new VwSystemEventConfiguration());
        modelBuilder.ApplyConfiguration(new DataLogConfiguration());
        modelBuilder.ApplyConfiguration(new LeadTicketNotificationLogConfiguration());
        modelBuilder.ApplyConfiguration(new DashboardNotificationConfiguration());
    }
}