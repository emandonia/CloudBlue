using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id)
            .HasName("PK_User");

        entity.HasIndex(e => new { e.CompanyId, e.BranchId }, "Users_CompanyId_BranchId_idx");
        entity.HasIndex(e => e.DepartmentId, "Users_DepartmentId_idx");
        entity.HasIndex(e => e.DirectManagerId, "Users_DirectManagerId_idx");
        entity.HasIndex(e => e.Email, "Users_Email_idx");
        entity.HasIndex(e => e.PositionId, "Users_PositionId_idx");
        entity.HasIndex(e => e.TopMostManagerId, "Users_TopMostManagerId_idx");
        entity.HasIndex(e => e.UserName, "Users_UserName_idx");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.BranchId)
            ;

        entity.Property(e => e.CompanyId)
            ;

        entity.Property(e => e.CreateDate)
            .HasPrecision(6);

        entity.Property(e => e.DepartmentId)
            ;

        entity.Property(e => e.DirectManagerId)
            ;

        entity.Property(e => e.DirectManagerName)
            .HasMaxLength(255);

        entity.Property(e => e.Email)
            .HasMaxLength(120);

        entity.Property(e => e.FailedPasswordAttemptCount)
            ;

        entity.Property(e => e.FullName)
            .HasMaxLength(250);

        entity.Property(e => e.HireDate)
            .HasPrecision(6);

        entity.Property(e => e.HireDateNumeric)
            ;

        entity.Property(e => e.InResaleTeam)
            ;

        entity.Property(e => e.IsApproved)
            ;

        entity.Property(e => e.IsBranchManager)
            ;

        entity.Property(e => e.IsLockedOut)
            ;

        entity.Property(e => e.IsParent)
            ;

        entity.Property(e => e.IsVirtual)
            ;

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

        entity.Property(e => e.LastPromotionDateNumeric)
            ;

        entity.Property(e => e.LastUpdateDate)
            .HasPrecision(6);

        entity.Property(e => e.ParentId)
            ;

        entity.Property(e => e.Password)
            .HasMaxLength(128);

        entity.Property(e => e.PasswordSalt)
            .HasMaxLength(200);

        entity.Property(e => e.PositionId)
            ;

        entity.Property(e => e.ResetPasswordKey)
            .HasMaxLength(128);

        entity.Property(e => e.ResignDate)
            .HasPrecision(6);

        entity.Property(e => e.ResignDateNumeric)
            ;

        entity.Property(e => e.TopMostManagerId)
            ;

        entity.Property(e => e.TopMostManagerName)
            .HasMaxLength(255);

        entity.Property(e => e.UserGroupId)
            ;

        entity.HasMany(z => z.UserPhones)
            .WithOne(z => z.User)
            .HasForeignKey(z => z.UserId)
            .HasPrincipalKey(z => z.Id);
        entity.Property(e => e.UserName)
            .HasMaxLength(120);
    }
}