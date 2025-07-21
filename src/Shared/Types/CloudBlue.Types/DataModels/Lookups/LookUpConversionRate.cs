namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpConversionRate : BaseDataModel<int>
{
    public int CurrencyId { get; set; }

    public decimal ConversionRateValue { get; set; }
}