using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class SalesPromotion
{
    public long Id { get; set; }

    public long SalesPersonId { get; set; }

    public int PositionId { get; set; }

    public DateTime PromotionStartDate { get; set; }

    public long PromotionStartDateNumeric { get; set; }

    public DateTime? PromotionEndDate { get; set; }

    public long PromotionEndDateNumeric { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime Created { get; set; }
}
