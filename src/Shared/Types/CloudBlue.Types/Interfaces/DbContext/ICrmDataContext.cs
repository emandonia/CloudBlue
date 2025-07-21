using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Domain.Interfaces.DbContext;

public interface ICrmDataContext
{
    DbSet<VwLeadTicket> VwLeadTickets { get; set; }
    DbSet<VwPrimeTcr> VwPrimeTcrs { get; set; }

    DbSet<Call> Calls { get; set; }

    DbSet<Client> Clients { get; set; }

    DbSet<ClientAddress> ClientAddresses { get; set; }

    DbSet<VwCall> VwCalls { get; set; }
    DbSet<PrimeTcr> PrimeTcrs { get; set; }
    DbSet<LeadTicket> LeadTickets { get; set; }
    DbSet<LeadTicketExtension> LeadTicketExtensions { get; set; }

    DbSet<ClientContactDevice> ClientContactDevices { get; set; }
    DbSet<VwClientLeadTicket> VwClientLeadTickets { get; set; }
    DbSet<PrimeTcrAttachment> PrimeTcrAttachments { get; set; }
    Task UpdateClientDevicesAsync(long clientId);

    int SaveChanges();
    Task<int> SaveChangesAsync();

    Task SaveBulkChangesAsync();
    void SetHighTimeOut();
}