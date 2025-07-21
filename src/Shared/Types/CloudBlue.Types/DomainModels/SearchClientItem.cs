using CloudBlue.Domain.DomainModels.Clients;

namespace CloudBlue.Domain.DomainModels;

public class SearchClientItem
{
    public bool Exist { get; set; }
    public string? Message { get; set; }
    public ClientItem ClientItem { get; set; } = new();
    public bool HasActiveLeads { get; set; }
    public bool CanAddLeadTicket { get; set; }
}