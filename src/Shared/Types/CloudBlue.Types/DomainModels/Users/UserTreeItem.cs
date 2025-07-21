namespace CloudBlue.Domain.DomainModels.Users;

public class UserTreeItem
{
    public bool ApplyTargets { get; set; }

    public bool IsManager { get; set; }

    public bool CanHaveTeam { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string? AgentName { get; set; }

    public int PositionId { get; set; }

    public bool InResaleTeam { get; set; }

    public int ResignDateNumeric { get; set; }

    public int HireDateNumeric { get; set; }

    public int DirectManagerId { get; set; }

    public int TopMostManagerId { get; set; }

    public int UserId { get; set; }

    public int ParentId { get; set; }
    public SalesUsersList SalesUsers { set; get; } = new();
}