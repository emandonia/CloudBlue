using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.App;

internal class DataLogConfiguration : IEntityTypeConfiguration<DataLog>
{
    public void Configure(EntityTypeBuilder<DataLog> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_DataLog");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.ActionDate)
            .HasPrecision(6);

        entity.Property(e => e.ActionDateNumeric)
            ;

        entity.Property(e => e.Action)
            .HasConversion<int>()
            .HasColumnName("ActionId");

        entity.Property(e => e.ActionStr)
            .HasMaxLength(100);

        entity.Property(e => e.Impersonated)
            ;

        entity.Property(e => e.IsNew)
            ;

        entity.Property(e => e.OriginalUserId)
            ;

        entity.Property(e => e.PageName)
            .HasMaxLength(100);

        entity.Property(e => e.UserId)
            ;

        entity.Property(e => e.UserName)
            .HasMaxLength(100);
    }
}