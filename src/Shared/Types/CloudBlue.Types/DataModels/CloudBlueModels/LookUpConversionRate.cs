using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LookUpConversionRate
{
    public int Id { get; set; }

    public int CurrencyId { get; set; }

    public decimal ConversionRateValue { get; set; }
}
