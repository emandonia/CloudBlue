using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class User
{
    public long Id { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string? FullName { get; set; }

    public long ParentId { get; set; }

    public bool IsParent { get; set; }

    public bool IsVirtual { get; set; }

    public int PositionId { get; set; }

    public DateTime? HireDate { get; set; }

    public bool InResaleTeam { get; set; }

    public DateTime? ResignDate { get; set; }

    public long ResignDateNumeric { get; set; }

    public long TopMostManagerId { get; set; }

    public string? TopMostManagerName { get; set; }

    public long HireDateNumeric { get; set; }

    public DateTime? LastPromotionDate { get; set; }

    public long LastPromotionDateNumeric { get; set; }

    public string? SalesForceUserId { get; set; }

    public string Password { get; set; } = null!;

    public string? UserName { get; set; }

    public string? ResetPasswordKey { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public string? Email { get; set; }

    public bool IsApproved { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LastPasswordChangedDate { get; set; }

    public DateTime? LastLockoutDate { get; set; }

    public int FailedPasswordAttemptCount { get; set; }

    public DateTime? LastPasswordFailureDate { get; set; }

    public bool IsBranchManager { get; set; }

    public int DepartmentId { get; set; }

    public int UserGroupId { get; set; }

    public long DirectManagerId { get; set; }
}
