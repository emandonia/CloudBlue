using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class VwUserConfiguration : IEntityTypeConfiguration<VwUser>
{
    public void Configure(EntityTypeBuilder<VwUser> entity)
    {
    entity.HasKey(z => z.Id);
    entity.ToView("VwUsers");

    entity.Property(e => e.BranchName)
        .HasMaxLength(120);

    entity.Property(e => e.CompanyName)
        .HasMaxLength(50);

    entity.Property(e => e.CreateDate)
        .HasPrecision(6);

    entity.Property(e => e.DepartmentName)
        .HasColumnType("character varying");

    entity.Property(e => e.DirectManagerName)
        .HasMaxLength(255);

    entity.Property(e => e.Email)
        .HasMaxLength(120);

    entity.Property(e => e.FullName)
        .HasMaxLength(250);

    entity.Property(e => e.FullNameLowered)
        .HasMaxLength(250);

    entity.Property(e => e.HireDate)
        .HasPrecision(6);

    entity.Property(e => e.LastLockoutDate)
        .HasPrecision(6);

    entity.Property(e => e.LastLoginDate)
        .HasPrecision(6);

    entity.Property(e => e.LastPasswordChangedDate)
        .HasPrecision(6);

    entity.Property(e => e.LastPasswordFailureDate)
        .HasPrecision(6);

    entity.Property(e => e.LastPromotionDate)
        .HasPrecision(6);

    entity.Property(e => e.LastUpdateDate)
        .HasPrecision(6);

    entity.Property(e => e.Password)
        .HasMaxLength(128);

    entity.Property(e => e.PasswordSalt)
        .HasMaxLength(200);

    entity.Property(e => e.ResetPasswordKey)
        .HasMaxLength(128);

    entity.Property(e => e.ResignDate)
        .HasPrecision(6);

    entity.Property(e => e.TopMostManagerName)
        .HasMaxLength(255);

    entity.Property(e => e.UserGroupName)
        .HasColumnType("character varying");

    entity.Property(e => e.UserName)
        .HasMaxLength(120);

    entity.HasMany(z => z.UserPhones)
        .WithOne(z => z.VwUser)
        .HasForeignKey(z => z.UserId)
        .HasPrincipalKey(z => z.Id);

    entity.Property(e => e.UserPositionName)
        .HasMaxLength(255);
    }
}