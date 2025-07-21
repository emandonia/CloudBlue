using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class LeadTicketExtensionConfiguration : IEntityTypeConfiguration<LeadTicketExtension>

{
    public void Configure(EntityTypeBuilder<LeadTicketExtension> entity)
    {
        entity.HasKey(e => e.LeadTicketId);

        entity.Property(e => e.AssociatedLeadId)
            ;

        entity.Property(e => e.ClientNameUpdated)
            ;

        entity.Property(e => e.ConvertedFromCall)
            ;

        entity.Property(e => e.ConvertedFromDummy)
            ;

        entity.Property(e => e.ConvertedFromReferral)
            ;

        entity.Property(e => e.CurrentAgentManagersTree)
            .HasMaxLength(1200);

        entity.Property(e => e.DuplicateLeadId)
            ;

        entity.Property(e => e.ExpRegId)
            ;

        entity.Property(e => e.ExtendedStatusId)
            ;

        entity.Property(e => e.IsCorporate)
            ;

        entity.Property(e => e.IsPureOld)
            ;

        entity.Property(e => e.IsReserved)
            ;

        entity.Property(e => e.LastConversionEventId)
            ;

        entity.Property(e => e.LastDeactivatedDateNumeric)
            ;

        entity.Property(e => e.LastRevivalDate)
            .HasPrecision(6);

        entity.Property(e => e.LastRevivalDateNumeric)
            ;

        entity.Property(e => e.LastVoidedDate)
            .HasPrecision(6);

        entity.Property(e => e.LastVoidedDateNumeric)
            ;

        entity.Property(e => e.OldAgentId)
            ;

        entity.Property(e => e.OldLeadRefId)
            ;

        entity.Property(e => e.OldStatusId)
            ;

        entity.Property(e => e.RejectDate)
            .HasPrecision(6);

        entity.Property(e => e.RejectReason)
            .HasMaxLength(1200);

        entity.Property(e => e.RevivedByAgentId)
            ;

        entity.Property(e => e.RevivedById)
            ;

        entity.Property(e => e.SalesForceLeadId)
            ;

        entity.Property(e => e.SettingOldLogId)
            ;

        entity.Property(e => e.ShowDummyToAgent)
            ;

        entity.Property(e => e.TeleSalesAgentId)
            ;

        entity.Property(e => e.VoidingLeadTicketId)
            ;

        entity.HasOne(z => z.LeadTicket)
            .WithOne(z => z.LeadTicketExtension)
            .HasForeignKey<LeadTicketExtension>(lte => lte.LeadTicketId)
            .HasPrincipalKey<LeadTicket>(z => z.Id);
    }
}