using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class FranchiseCompany
{
    public int Id { get; set; }

    public int CountryId { get; set; }

    public string? CompanyName { get; set; }

    public bool IsInsider { get; set; }

    public virtual ICollection<FranchiseBranch> FranchiseBranches { get; set; } = new List<FranchiseBranch>();
}
