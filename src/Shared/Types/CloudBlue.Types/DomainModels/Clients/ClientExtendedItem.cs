using CloudBlue.Domain.DomainModels.CallLeads;

namespace CloudBlue.Domain.DomainModels.Clients;

public class ClientExtendedItem : ClientItem
{
    public List<LeadTicketBriefInfoItem> LeadTickets { get; set; } = new();
}