namespace CloudBlue.Domain.DomainModels.Users.UsersManagement;

public class UserItemForList
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string FullName { get; set; } = null!;

    public bool IsVirtual { get; set; }

    public int PositionId { get; set; }

    public DateTime? HireDate { get; set; }

    public bool InResaleTeam { get; set; }

    public DateTime? ResignDate { get; set; }

    public int TopMostManagerId { get; set; }

    public string? TopMostManagerName { get; set; }

    public DateTime? LastPromotionDate { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsApproved { get; set; }

    public bool IsLockedOut { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public DateTime? LastLockoutDate { get; set; }

    public DateTime? LastPasswordFailureDate { get; set; }

    public int DirectManagerId { get; set; }

    public string? DirectManagerName { get; set; }

    public string? UserPositionName { get; set; }

    public string? DepartmentName { get; set; }

    public string? BranchName { get; set; }

    public string? CompanyName { get; set; }
    public int DepartmentId { get; set; }
    public string? AreaCode { get; set; }
    public string? Phone { get; set; }
    public string? DeviceInfo { get; set; } = string.Empty;
    public long DeviceId { get; set; }
    public bool CanAccessCommissionSystem { get; set; }
    public bool CanUserAccessPortal { get; set; }


}