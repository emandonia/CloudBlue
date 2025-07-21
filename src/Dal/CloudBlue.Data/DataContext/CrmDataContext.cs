using CloudBlue.Data.Configurations.App;
using CloudBlue.Data.Configurations.Crm;
using CloudBlue.Data.Configurations.Users;
using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.DataContext;
#pragma warning disable S3881
public class CrmDataContext : DbContext, ICrmDataContext
#pragma warning restore S3881
{
    public CrmDataContext()
    {
    }

    public async Task SaveBulkChangesAsync()
    {
        await this.BulkSaveChangesAsync();
    }

    public void SetHighTimeOut()
    {
        Database.SetCommandTimeout(TimeSpan.FromMinutes(30));

    }

    public CrmDataContext(DbContextOptions<CrmDataContext> options) : base(options)
    {
    }

    public DbSet<SystemEventTemplate> SystemEventTemplates { get; set; }
    public DbSet<SystemEvent> SystemEvents { get; set; }
    public DbSet<DataLog> DataLogs { get; set; }
    public DbSet<PrimeTcr> PrimeTcrs { get; set; }

    public DbSet<VwLeadTicket> VwLeadTickets { get; set; }
    public DbSet<VwPrimeTcr> VwPrimeTcrs { get; set; }
    public DbSet<Call> Calls { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientAddress> ClientAddresses { get; set; }
    public DbSet<VwCall> VwCalls { get; set; }
    public DbSet<LeadTicket> LeadTickets { get; set; }
    public DbSet<LeadTicketExtension> LeadTicketExtensions { get; set; }

    public DbSet<ClientContactDevice> ClientContactDevices { get; set; }
    public DbSet<VwClientLeadTicket> VwClientLeadTickets { get; set; }
    public DbSet<PrimeTcrAttachment> PrimeTcrAttachments { get; set; }

    public async Task UpdateClientDevicesAsync(long clientId)
    {
        await Database.ExecuteSqlRawAsync("CALL prc_update_client_devices({0})", clientId);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CallConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ClientContactDeviceConfiguration());
        modelBuilder.ApplyConfiguration(new LeadTicketConfiguration());
        modelBuilder.ApplyConfiguration(new LeadTicketExtensionConfiguration());
        modelBuilder.ApplyConfiguration(new SystemEventConfiguration());
        modelBuilder.ApplyConfiguration(new VwAgentConfiguration());
        modelBuilder.ApplyConfiguration(new VwCallConfiguration());
        modelBuilder.ApplyConfiguration(new VwClientLeadTicketConfiguration());
        modelBuilder.ApplyConfiguration(new VwSystemEventConfiguration());
        modelBuilder.ApplyConfiguration(new DataLogConfiguration());
        modelBuilder.ApplyConfiguration(new PrimeTcrConfiguration());
        modelBuilder.ApplyConfiguration(new VwPrimeTcrConfiguration());
    }
}