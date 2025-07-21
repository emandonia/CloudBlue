namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpCurrency : BaseDataModel<int>
{
    public string Currency { get; set; } = null!;

    public string CurrencySymbol { get; set; } = null!;

    public bool IsDefault { get; set; }
}