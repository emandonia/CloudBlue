using System.ComponentModel.DataAnnotations.Schema;

namespace CloudBlue.Domain.DataModels.Tenants;

[Table("FranchiseBranches")]
public class FranchiseBranch
{
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public bool Disabled { get; set; }
    public bool PropertyAutoApproval { get; set; }
    public bool RequestAutoApproval { get; set; }

    public string BranchName { get; set; } = null!;
    public string? FillColor { get; set; }
    public string? TextColor { get; set; }

    public virtual FranchiseCompany Company { get; set; } = null!;
}