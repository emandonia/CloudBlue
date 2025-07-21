namespace CloudBlue.Domain.DataModels.Lookups;

public class LookUpOutsideBroker : BaseDataModel<int>
{
    public string OutsideBroker { set; get; } = null!;
    public decimal OutsideBrokerCommissionPercentage { set; get; }
}