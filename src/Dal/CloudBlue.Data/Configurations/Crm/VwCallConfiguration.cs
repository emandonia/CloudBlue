using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class VwCallConfiguration : IEntityTypeConfiguration<VwCall>
{
    public void Configure(EntityTypeBuilder<VwCall> entity)
    {
    entity.HasKey(z => z.Id);
    entity.ToView("VwCalls");

    entity.Property(e => e.BranchName)
        .HasMaxLength(120);

    entity.Property(e => e.CallNote)
        .HasMaxLength(2500);

    entity.Property(e => e.CallStatus)
        .HasMaxLength(50);

    entity.Property(e => e.CallStatusId)
        .HasConversion<int>();

    entity.Property(e => e.CallTypeId)
        .HasConversion<int>();

    entity.Property(e => e.CallType)
        .HasMaxLength(50);

    entity.Property(e => e.CallTypeOther)
        .HasMaxLength(150);

    entity.Property(e => e.CanceledBy)
        .HasMaxLength(60);

    entity.Property(e => e.ClientName)
        .HasMaxLength(150);

    entity.Property(e => e.ClientTitle)
        .HasMaxLength(50);

    entity.Property(e => e.ClientType)
        .HasMaxLength(50);

    entity.Property(e => e.CompanyName)
        .HasMaxLength(50);

    entity.Property(e => e.ContactDevicesJsonb)
        .HasColumnType("jsonb");

    entity.Property(e => e.CreatedBy)
        .HasMaxLength(60);

    entity.Property(e => e.DurationInSeconds)
        .HasColumnName("durationInSeconds");

    entity.Property(e => e.DurationStr)
        .HasMaxLength(30);

    entity.Property(e => e.HandledBy)
        .HasMaxLength(60);

    entity.Property(e => e.KnowSource)
        .HasMaxLength(50);

    entity.Property(e => e.KnowSourceExtra)
        .HasMaxLength(250);

    entity.Property(e => e.KnowSubSource)
        .HasMaxLength(255);

    entity.Property(e => e.LeadSource)
        .HasMaxLength(50);

    entity.Property(e => e.Location)
        .HasMaxLength(500);

    entity.Property(e => e.PropertyType)
        .HasMaxLength(150);

    entity.Property(e => e.RecentEventsJsonb)
        .HasColumnType("jsonb");

    entity.Property(e => e.SourceExtra)
        .HasMaxLength(250);

    entity.Property(e => e.StatusReason)
        .HasMaxLength(2000);
    }
}