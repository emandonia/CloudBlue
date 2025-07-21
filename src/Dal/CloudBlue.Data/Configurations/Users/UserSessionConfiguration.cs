using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> entity)
    {
        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.ApiKey)
            .HasMaxLength(120);

        entity.Property(e => e.BranchId)
            ;

        entity.Property(e => e.CompanyId)
            ;

        entity.Property(e => e.CreationDate)
            .HasPrecision(6);

        entity.Property(e => e.DeviceServiceId)
            .HasMaxLength(120);

        entity.Property(e => e.ExpireDate)
            .HasPrecision(6);

        entity.Property(e => e.ExpireDateNumeric)
            ;

        entity.Property(e => e.IsExpired)
            ;

        entity.Property(e => e.LoginProvider)
            .HasMaxLength(120);

        entity.Property(e => e.SerializedObject)
            .HasColumnType("json");

        entity.Property(e => e.SetExpiredOn)
            .HasPrecision(6);

        entity.Property(e => e.UserId)
            ;
    }
}