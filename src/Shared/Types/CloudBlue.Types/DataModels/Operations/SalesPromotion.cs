namespace CloudBlue.Domain.DataModels.Operations;

public class SalesPromotion : BaseDataModel<long>
{
    public int SalesPersonId { get; set; }

    public int PositionId { get; set; }

    public DateTime PromotionStartDate { get; set; }

    public int PromotionStartDateNumeric { get; set; }

    public DateTime? PromotionEndDate { get; set; }

    public int PromotionEndDateNumeric { get; set; }

    public DateTime? LastUpdated { get; set; }

    public DateTime Created { get; set; }
}