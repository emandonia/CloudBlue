using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class PrivilegeConfiguration : IEntityTypeConfiguration<Privilege>
{
    public void Configure(EntityTypeBuilder<Privilege> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_userright");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.ActionName)
            .HasMaxLength(255);

        entity.Property(e => e.ControllerName)
            .HasMaxLength(255);

        entity.Property(e => e.Path)
            .HasMaxLength(255);

        entity.Property(e => e.PrivilegeMetaData)
            .HasMaxLength(255);

        entity.Property(e => e.PrivilegeName)
            .HasMaxLength(300);

        entity.HasOne(d => d.PrivilegeCategory)
            .WithMany(p => p.Privileges)
            .HasForeignKey(d => d.PrivilegeCategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK__Privilege__Privi__0697FACD");
    }
}