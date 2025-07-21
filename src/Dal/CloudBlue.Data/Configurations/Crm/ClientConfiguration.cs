using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK__Contact__5C66CEEBE587CD38");

        entity.HasIndex(e => e.ClientNameLowered, "Clients_ClientNameLowered_idx");
        entity.HasIndex(e => e.ClientCategoryId, "Clients_ClientCategoryId_idx");
        entity.HasIndex(e => e.ClientNameArabic, "Clients_ClientNameAR_idx");
        entity.HasIndex(e => e.ClientTitleId, "Clients_ClientTitleId_idx");
        entity.HasIndex(e => e.ClientTypeId, "Clients_ClientTypeId_idx");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.ClientCategoryId)
            ;

        entity.Property(e => e.ClientCompanyName)
            .HasMaxLength(150);

        entity.Property(e => e.ClientName)
            .HasMaxLength(150);

        entity.Property(e => e.ClientNameArabic)
            .HasMaxLength(150)
            .HasColumnName("ClientNameArabic");

        entity.Property(e => e.ClientOccupation)
            .HasColumnType("character varying");

        entity.Property(e => e.ClientOccupationFieldId)
            ;

        entity.Property(e => e.ClientTitleId)
            ;

        entity.Property(e => e.ClientTypeId)
            ;

        entity.Property(e => e.CompanyId)
            ;

        entity.Property(e => e.ContactDevicesJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.CreationDate)
            .HasPrecision(6);

        entity.Property(e => e.CreationDateNumeric)
            ;

        entity.Property(e => e.CreatedById)
            ;

        entity.Property(e => e.GenderId)
            ;

        entity.Property(e => e.IsOptedOut)
            ;

        entity.Property(e => e.IsPotential)
            ;

        entity.Property(e => e.IsVip)
            ;

        entity.Property(e => e.LastEditDate)
            .HasColumnType("timestamp(0) without time zone");

        entity.Property(e => e.LastEditDateNumeric)
            ;

        entity.Property(e => e.LastEditorId)
            ;

        entity.Property(e => e.Notes)
            .HasMaxLength(2500);

        entity.Property(e => e.WebLeadId)
            ;
    }
}