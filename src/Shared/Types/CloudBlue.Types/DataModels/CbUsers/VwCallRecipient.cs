namespace CloudBlue.Domain.DataModels.CbUsers;

public class VwCallRecipient
{
    public int UserId { get; set; }

    public int? CompanyId { get; set; }

    public int? BranchId { get; set; }

    public string UserFullName { get; set; } = null!;

    public int? PositionId { get; set; }

    public int? UserGroupId { get; set; }
}