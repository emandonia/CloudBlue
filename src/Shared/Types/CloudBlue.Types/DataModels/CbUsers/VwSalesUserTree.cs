namespace CloudBlue.Domain.DataModels.CbUsers;

public class VwSalesUserTree : BaseDataModel<int>
{
    public string? UserPositionName { get; set; }

    public bool ApplyTargets { get; set; }

    public bool IsManager { get; set; }

    public bool CanHaveTeam { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string? FullName { get; set; }

    public int PositionId { get; set; }

    public DateTime? HireDate { get; set; }

    public DateTime? ResignDate { get; set; }

    public bool InResaleTeam { get; set; }

    public int ResignDateNumeric { get; set; }

    public int HireDateNumeric { get; set; }

    public int DirectManagerId { get; set; }

    public int TopMostManagerId { get; set; }

    public int UserId { get; set; }

    public int ParentId { get; set; }
}