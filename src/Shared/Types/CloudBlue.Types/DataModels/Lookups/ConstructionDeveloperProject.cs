namespace CloudBlue.Domain.DataModels.Lookups;

public class ConstructionDeveloperProject : BaseDataModel<int>
{
    public string ProjectName { get; set; } = null!;

    public decimal CompanyRevenuePercentage { get; set; }

    public int DeveloperId { get; set; }

    public bool IsRegular { get; set; }

    public bool ForceHalfDeal { get; set; }

    public decimal ForceAchievementPercentage { get; set; }

    public bool ForceFlatRateCommission { get; set; }

    public decimal ForceCommissionPercentage { get; set; }

    public bool Selection { get; set; }

    public virtual ConstructionDeveloper Developer { get; set; } = null!;
}