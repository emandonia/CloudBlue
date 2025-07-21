using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class PrivilegeCategoryConfiguration : IEntityTypeConfiguration<PrivilegeCategory>
{
    public void Configure(EntityTypeBuilder<PrivilegeCategory> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK__Privileg__3214EC079D9D5728");

        entity.Property(e => e.Id)
            .ValueGeneratedNever();

        entity.Property(e => e.PrivilegeCategoryName)
            .HasColumnType("character varying");
    }
}