using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.App;

internal class SystemEventConfiguration : IEntityTypeConfiguration<SystemEvent>

{
    public void Configure(EntityTypeBuilder<SystemEvent> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("SystemEvents_pkey");

        entity.HasIndex(e => e.ContactingTypeId, "SystemEvents_ContactingTypeId_idx");
        entity.HasIndex(e => new { e.EntityTypeId, e.EntityId }, "SystemEvents_EntityTypeId_EntityId_idx");
        entity.HasIndex(e => e.EventCreationDateTimeNumeric, "SystemEvents_EventCreationDateTimeNumeric_idx");
        entity.HasIndex(e => e.EventProcessId, "SystemEvents_EventProcessId_idx");
        entity.HasIndex(e => e.EventTypeId, "SystemEvents_EventTypeId_idx");
        entity.HasIndex(e => e.UserId, "SystemEvents_UserId_idx");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.ClientId)
            ;

        entity.Property(e => e.ContactingTypeId)
            ;

        entity.Property(e => e.Dismissed)
            ;

        entity.Property(e => e.EntityId)
            ;

        entity.Property(e => e.EntityTypeId)
            ;

        entity.Property(e => e.EventComment)
            .HasMaxLength(4000);

        entity.Property(e => e.EventCreationDateTime)
            .HasPrecision(6);

        entity.Property(e => e.EventCreationDateTimeNumeric)
            ;

        entity.Property(e => e.EventDateTime)
            .HasPrecision(6);

        entity.Property(e => e.EventDateTimeNumeric)
            ;

        entity.Property(e => e.EventProcessId)
            ;

        entity.Property(e => e.EventTypeId)
            ;

        entity.Property(e => e.FromUserId)
            ;

        entity.Property(e => e.Impersonated)
            ;

        entity.Property(e => e.IsConverted)
            ;

        entity.Property(e => e.OriginalUserId)
            ;

        entity.Property(e => e.ToUserId)
            ;

        entity.Property(e => e.UserId)
            ;
    }
}