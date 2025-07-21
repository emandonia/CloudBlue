namespace CloudBlue.Domain.DataModels.CbUsers;

public class User : BaseDataModel<int>
{
    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int[] ManagersIdsArray { get; set; } = [];

    public string FullName { get; set; } = null!;
    public string FullNameLowered { get; set; } = null!;

    public int ParentId { get; set; }

    public bool CanUserAccessPortal { get; set; }
    public bool CanAccessCommissionSystem { get; set; }
    public bool IsParent { get; set; }

    public bool IsVirtual { get; set; }

    public int PositionId { get; set; }

    public DateTime? HireDate { get; set; }

    public bool InResaleTeam { get; set; }

    public DateTime? ResignDate { get; set; }

    public int ResignDateNumeric { get; set; }

    public int TopMostManagerId { get; set; }

    public string? TopMostManagerName { get; set; }

    public int HireDateNumeric { get; set; }

    public DateTime? LastPromotionDate { get; set; }

    public int LastPromotionDateNumeric { get; set; }

    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? ResetPasswordKey { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public string Email { get; set; } = null!;

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
    public int[] TeamsIds { get; set; } = [];

    public int UserGroupId { get; set; }

    public int DirectManagerId { get; set; }

    public string? DirectManagerName { get; set; }

    public virtual ICollection<UserPhone> UserPhones { get; set; } = new List<UserPhone>();
}