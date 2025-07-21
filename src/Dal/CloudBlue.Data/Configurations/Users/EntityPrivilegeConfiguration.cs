using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class EntityPrivilegeConfiguration : IEntityTypeConfiguration<EntityPrivilege>
{
    public void Configure(EntityTypeBuilder<EntityPrivilege> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_userrightToGroup");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.HasOne(d => d.PrivilegeEntityType)
            .WithMany(p => p.EntityPrivileges)
            .HasForeignKey(d => d.PrivilegeEntityTypeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("EntityPrivileges_PrivilegeEntityTypeId_fkey");

        entity.HasOne(d => d.Privilege)
            .WithMany(p => p.EntityPrivileges)
            .HasForeignKey(d => d.PrivilegeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("EntityPrivileges_PrivilegeId_fkey");

        entity.HasOne(d => d.PrivilegeScope)
            .WithMany(p => p.EntityPrivileges)
            .HasForeignKey(d => d.PrivilegeScopeId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK__EntityPri__Privi__1209AD79");
    }
}