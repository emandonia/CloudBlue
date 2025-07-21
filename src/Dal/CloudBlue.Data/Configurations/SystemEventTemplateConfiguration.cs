using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations;

internal class SystemEventTemplateConfiguration : IEntityTypeConfiguration<SystemEventTemplate>
{
    public void Configure(EntityTypeBuilder<SystemEventTemplate> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("SystemEventTemplates_pkey");

        entity.Property(e => e.AnchorsJson)
            .HasColumnType("json");

        entity.Property(e => e.EntityTypeId)
            ;

        entity.Property(e => e.EventProcessId)
            ;

        entity.Property(e => e.EventTemplate)
            .HasMaxLength(2000);

        entity.Property(e => e.EventTypeId)
            ;

        entity.Property(e => e.TemplateName)
            .HasMaxLength(255);
    }
}