using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class PrivilegeScopeConfiguration : IEntityTypeConfiguration<PrivilegeScope>
{
    public void Configure(EntityTypeBuilder<PrivilegeScope> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK__Privileg__3214EC0709C309AF");

        entity.Property(e => e.Id)
            .ValueGeneratedNever();

        entity.Property(e => e.PrivilegeScopeName)
            .HasMaxLength(255);
    }
}