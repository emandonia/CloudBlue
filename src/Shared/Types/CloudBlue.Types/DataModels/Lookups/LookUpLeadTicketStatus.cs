namespace CloudBlue.Domain.DataModels.Lookups;

public sealed class LookUpLeadTicketStatus : BaseDataModel<int>
{
    public int DisplayOrder { get; set; }

    public string LeadTicketStatus { get; set; } = null!;
}