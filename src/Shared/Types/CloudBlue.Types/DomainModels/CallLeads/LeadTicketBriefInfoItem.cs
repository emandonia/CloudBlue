using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadTicketBriefInfoItem
{
    public long LeadTicketId { get; set; }
    public long ClientId { get; set; }

    public int CompanyId { get; set; }
    public bool IsFullLeadTicket { get; set; }
    public LeadTicketStatuses? LeadTicketStatus { get; set; }
    public ProspectStatuses? ProspectStatus { get; set; }
    public string? CompanyName { get; set; }
    public int BranchId { get; set; }
    public int AgentId { set; get; }
    public string? BranchName { get; set; }
    public string? AgentName { get; set; }
    public long CreationDateNumeric { get; set; }
}