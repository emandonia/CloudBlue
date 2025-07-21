using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class PrivilegeEntityTypeConfiguration : IEntityTypeConfiguration<PrivilegeEntityType>
{
    public void Configure(EntityTypeBuilder<PrivilegeEntityType> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK__PrivilegeEntityType__3214EC079D9D5728");

        entity.Property(e => e.Id)
            .ValueGeneratedNever();

        entity.Property(e => e.PrivilegeEntityTypeName)
            .HasColumnType("character varying")
            .HasColumnName("PrivilegeEntityType");
    }
}