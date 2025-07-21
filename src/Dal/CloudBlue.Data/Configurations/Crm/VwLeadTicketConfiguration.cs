using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class VwLeadTicketConfiguration : IEntityTypeConfiguration<VwLeadTicket>
{
    public void Configure(EntityTypeBuilder<VwLeadTicket> entity)
    {
        entity.HasKey(z => z.Id);
        entity.ToView("VwLeadTickets");

        entity.Property(e => e.ActivityStatsJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.LastAssignedDate)
            .HasPrecision(6);

        entity.Property(e => e.CallBackDate)
            .HasPrecision(6);

        entity.Property(e => e.CreationDate)
            .HasPrecision(6);

        entity.Property(e => e.ExtendedDate)
            .HasPrecision(6);

        entity.Property(e => e.BranchName)
            .HasMaxLength(120);

        entity.Property(e => e.BudgetFrom)
            .HasPrecision(18, 2);

        entity.Property(e => e.BudgetTo)
            .HasPrecision(18, 2);

        entity.Property(e => e.ClientName)
            .HasMaxLength(150);

        entity.Property(e => e.ClientNameArabic)
            .HasMaxLength(150);

        entity.Property(e => e.ClientTitle)
            .HasMaxLength(50);

        entity.Property(e => e.LeadTicketStatusId);
        entity.Property(e => e.ServiceId);
        entity.Property(e => e.SalesTypeId);

        entity.Property(e => e.ClientType)
            .HasMaxLength(50);

        entity.Property(e => e.CompanyName)
            .HasMaxLength(50);

        entity.Property(e => e.RecentEventsJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.CorporateCompany)
            .HasMaxLength(255);

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(120);

        entity.Property(e => e.CreationDate)
            .HasPrecision(6);

        entity.Property(e => e.Currency)
            .HasMaxLength(50);

        entity.Property(e => e.CurrencySymbol)
            .HasMaxLength(6);

        entity.Property(e => e.CurrentAgent)
            .HasMaxLength(255);

        entity.Property(e => e.FormName)
            .HasMaxLength(1200);

        entity.Property(e => e.KnowSource)
            .HasMaxLength(50);

        entity.Property(e => e.KnowSubSource)
            .HasMaxLength(255);

        entity.Property(e => e.LeadSource)
            .HasMaxLength(50);

        entity.Property(e => e.StatusBackgroundColor)
            .HasMaxLength(255);

        entity.Property(e => e.StatusFontColor)
            .HasMaxLength(255);

        entity.Property(e => e.LeadTicketNote)
            .HasMaxLength(2500);

        entity.Property(e => e.LeadTicketStatus)
            .HasMaxLength(50);

        entity.Property(e => e.Location)
            .HasMaxLength(1200);

        entity.Property(e => e.MarketingAgency)
            .HasMaxLength(255);

        entity.Property(e => e.PropertyType)
            .HasMaxLength(150);

        entity.Property(e => e.SalesType)
            .HasMaxLength(50);

        entity.Property(e => e.Service)
            .HasMaxLength(100);

        entity.Property(e => e.Usage)
            .HasMaxLength(150);
    }
}