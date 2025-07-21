using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.App;

internal class LeadTicketNotificationLogConfiguration : IEntityTypeConfiguration<LeadTicketNotificationLog>
{
    public void Configure(EntityTypeBuilder<LeadTicketNotificationLog> entity)
    {
        entity.HasKey(e => e.Id);

        entity.ToTable("LeadTicketNotificationLogs");

        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.RecipientEmail).HasMaxLength(255);
        entity.Property(e => e.RecipientPhone).HasMaxLength(30);
        entity.Property(e => e.SendingDate).HasPrecision(6);
        entity.Property(e => e.SendingResponse).HasMaxLength(5000);
        entity.Property(e => e.SentMessage).HasMaxLength(5000);
        entity.Property(e => e.SentMessageId).HasMaxLength(150);
    }
}