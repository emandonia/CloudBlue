using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class FranchiseBranch
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? BranchName { get; set; }

    public string? FillColor { get; set; }

    public string? TextColor { get; set; }

    public bool Disabled { get; set; }

    public bool? PropertyAutoAproval { get; set; }

    public bool? RequestAutoAproval { get; set; }

    public virtual FranchiseCompany? Company { get; set; }
}
