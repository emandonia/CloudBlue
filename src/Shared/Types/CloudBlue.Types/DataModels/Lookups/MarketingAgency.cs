namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class MarketingAgency : BaseDataModel<int>
{
    public string Agency { get; set; } = null!;
}