using System.ComponentModel.DataAnnotations.Schema;

namespace CloudBlue.Domain.DataModels.Tenants;

[Table("FranchiseCompanies")]
public class FranchiseCompany : BaseDataModel<int>
{
    public string CompanyName { get; set; } = null!;

    public bool IsInsider { get; set; }

    public virtual ICollection<FranchiseBranch> Branches { get; set; } = new List<FranchiseBranch>();
    public int CountryId { get; set; }
}