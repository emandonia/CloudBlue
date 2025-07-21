namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadTicketInfoItemForTcr
{
    public long LeadTicketId { get; set; }
    public long ClientId { get; set; }
    public string? ClientName { set; get; }
    public int PropertyTypeId { set; get; }
    public int UsageId { set; get; }
    public int SalesTypeId { set; get; }
    public int CompanyId { get; set; }
    public int LeadTicketStatusId { get; set; }
    public string? CompanyName { get; set; }
    public int BranchId { get; set; }
    public int AgentId { set; get; }
    public string? BranchName { get; set; }
    public string? AgentName { get; set; }
    public int ServiceTypeId { get; set; }
    public string? KnowSource { get; set; }
    public string? KnowSubSource { get; set; }
    public int KnowSourceId { get; set; }
    public int KnowSubSourceId { get; set; }
    public bool IsCorporate { get; set; }
    public string? Usage { get; set; }
    public string? LeadSource { get; set; }
    public string? TeleSalesAgentName { get; set; }
    public int TeleSalesAgentId { get; set; }
}