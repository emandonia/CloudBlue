using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels.Crm;

public class VwClientLeadTicket
{
    public long LeadTicketId { get; set; }

    public long ClientId { get; set; }

    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public string? CompanyName { get; set; }

    public LeadTicketStatuses? LeadTicketStatus { get; set; }

    public int CompanyId { get; set; }

    public bool IsFullLeadTicket { get; set; }

    public int CreationDateNumeric { get; set; }

    public int AgentId { get; set; }

    public string? AgentName { get; set; }

    public ProspectStatuses? ProspectStatus { get; set; }

    public bool IsOld { get; set; }

    public int TcrStatusId { get; set; }

    public bool IsVoided { get; set; }
}