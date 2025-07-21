using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.App;

internal class VwSystemEventConfiguration : IEntityTypeConfiguration<VwSystemEvent>
{
    public void Configure(EntityTypeBuilder<VwSystemEvent> entity)
    {
        entity.HasKey(z => z.Id);
        entity.ToView("VwSystemEvents");

        entity.Property(e => e.ClientName)
            .HasMaxLength(150);

        entity.Property(e => e.ContactingType)
            .HasMaxLength(60);

        entity.Property(e => e.EventComment)
            .HasMaxLength(4000);

        entity.Property(e => e.EventCreationDateTime)
            .HasPrecision(6);

        entity.Property(e => e.EventDateTime)
            .HasPrecision(6);

        entity.Property(e => e.EventProcess)
            .HasMaxLength(50);

        entity.Property(e => e.EventType)
            .HasMaxLength(50);

        entity.Property(e => e.FullName)
            .HasMaxLength(250);
    }
}