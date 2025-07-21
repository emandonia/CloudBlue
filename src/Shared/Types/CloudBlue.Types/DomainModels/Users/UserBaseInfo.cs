namespace CloudBlue.Domain.DomainModels.Users;

public class UserBaseInfo
{
    // Ids for edit user
    public int Id { get; set; }
    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string? FullName { get; set; }

    public int DepartmentId { get; set; }

    public int ParentId { get; set; }

    public int PositionId { get; set; }

    public string? Email { get; set; }

    public bool IsApproved { get; set; }

    public bool IsLockedOut { get; set; }
}