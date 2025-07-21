using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class VwEntityPrivilegeConfiguration : IEntityTypeConfiguration<VwEntityPrivilege>
{
    public void Configure(EntityTypeBuilder<VwEntityPrivilege> entity)
    {
        entity.HasKey(z => z.Id);
        entity.ToView("VwEntityPrivileges");

        entity.Property(e => e.ActionName)
            .HasMaxLength(255);

        entity.Property(e => e.ControllerName)
            .HasMaxLength(255);

        entity.Property(e => e.Path)
            .HasMaxLength(255);

        entity.Property(e => e.PrivilegeCategoryName)
            .HasColumnType("character varying");

        entity.Property(e => e.PrivilegeEntityTypeName)
            .HasColumnType("character varying");

        entity.Property(e => e.PrivilegeMetaData)
            .HasMaxLength(255);

        entity.Property(e => e.PrivilegeName)
            .HasMaxLength(300);

        entity.Property(e => e.PrivilegeScopeName)
            .HasMaxLength(255);

        entity.Property(e => e.Privilege)
            .HasColumnName("PrivilegeId")
            .HasConversion<int>();

        entity.Property(e => e.PrivilegeCategory)
            .HasColumnName("PrivilegeCategoryId")
            .HasConversion<int>();

        entity.Property(e => e.PrivilegeEntityType)
            .HasColumnName("PrivilegeEntityTypeId")
            .HasConversion<int>();

        entity.Property(e => e.PrivilegeScope)
            .HasColumnName("PrivilegeScopeId")
            .HasConversion<int>();
    }
}