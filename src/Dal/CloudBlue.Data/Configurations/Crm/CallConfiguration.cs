using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class CallConfiguration : IEntityTypeConfiguration<Call>
{
    public void Configure(EntityTypeBuilder<Call> entity)
    {
    entity.HasKey(e => e.Id)
        .HasName("Calls_pkey");

    entity.HasIndex(e => e.BranchId, "Calls_BranchId_idx");
    entity.HasIndex(e => e.CallStatus, "Calls_CallStatusId_idx");
    entity.HasIndex(e => e.CallType, "Calls_CallTypeId_idx");

    entity.Property(e => e.CallType)
        .HasConversion<int>()
        .HasColumnName("CallTypeId");

    entity.HasIndex(e => e.ClientId, "Calls_ClientId_idx");
    entity.HasIndex(e => e.CompanyId, "Calls_CompanyId_idx");
    entity.HasIndex(e => e.CreatedById, "Calls_CreatedById_idx");
    entity.HasIndex(e => e.CreationDateNumeric, "Calls_CreationDateNumeric_idx");
    entity.HasIndex(e => e.KnowSourceExtraId, "Calls_KnowSourceExtraId_idx");
    entity.HasIndex(e => e.KnowSourceId, "Calls_KnowSourceId_idx");
    entity.HasIndex(e => e.LeadSourceId, "Calls_LeadSourceId_idx");

    entity.Property(e => e.Id)
        .UseIdentityAlwaysColumn();

    entity.Property(e => e.BranchId)
        ;

    entity.Property(e => e.CallNote)
        .HasMaxLength(2500);

    entity.Property(e => e.CallStatus)
        .HasConversion<int>()
        .HasColumnName("CallStatusId");

    entity.Property(e => e.CallTypeOther)
        .HasMaxLength(150);

    entity.Property(e => e.CampaignOwnerId)
        ;

    entity.Property(e => e.CanceledBy)
        .HasMaxLength(60);

    entity.Property(e => e.CanceledById)
        ;

    entity.Property(e => e.CanceledDate)
        .HasPrecision(6);

    entity.Property(e => e.CanceledDateNumeric)
        ;

    entity.Property(e => e.ClientId)
        ;

    entity.Property(e => e.CollectiveCampaignId)
        ;

    entity.Property(e => e.CompanyId)
        ;

    entity.Property(e => e.CreatedBy)
        .HasMaxLength(60);

    entity.Property(e => e.CreatedById)
        ;

    entity.Property(e => e.CreationDate)
        .HasPrecision(6);

    entity.Property(e => e.CreationDateNumeric)
        ;

    entity.Property(e => e.DurationInSeconds)
        
        .HasColumnName("durationInSeconds");

    entity.Property(e => e.DurationStr)
        .HasMaxLength(30);

    entity.Property(e => e.ExpRegId)
        ;

    entity.Property(e => e.HandledBy)
        .HasMaxLength(60);

    entity.Property(e => e.HandledById)
        ;

    entity.Property(e => e.HandledDate)
        .HasPrecision(6);

    entity.Property(e => e.HandledDateNumeric)
        ;

    entity.Property(e => e.IsArchived)
        ;

    entity.Property(e => e.KnowDate)
        .HasPrecision(6);

    entity.Property(e => e.KnowSourceExtra)
        .HasMaxLength(250);

    entity.Property(e => e.KnowSourceExtraId)
        ;

    entity.Property(e => e.KnowSourceId)
        ;

    entity.Property(e => e.LastEventId)
        ;

    entity.Property(e => e.LeadSourceId)
        ;

    entity.Property(e => e.Location)
        .HasMaxLength(500);

    entity.Property(e => e.ProjectCampaignId)
        ;

    entity.Property(e => e.RecentEventsJsonb)
        .HasColumnType("jsonb");

    entity.Property(e => e.SourceExtra)
        .HasMaxLength(250);

    entity.Property(e => e.StatusReason)
        .HasMaxLength(2000);

    entity.Property(e => e.VoidReasonId)
        ;

    entity.Property(e => e.WebLeadId)
        ;
    }
}