using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class VwClientLeadTicketConfiguration : IEntityTypeConfiguration<VwClientLeadTicket>
{
    public void Configure(EntityTypeBuilder<VwClientLeadTicket> entity)
    {
        entity.HasKey(z => z.LeadTicketId);
        entity.ToView("VwClientLeadTickets");

        entity.Property(e => e.AgentName)
            .HasMaxLength(255);

        entity.Property(e => e.BranchName)
            .HasMaxLength(120);

        entity.Property(e => e.CompanyName)
            .HasMaxLength(50);
    }
}