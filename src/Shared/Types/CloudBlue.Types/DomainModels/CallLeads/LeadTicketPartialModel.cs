using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadTicketPartialModel
{
    // public int AgencyId { set; get; }
    [Required(ErrorMessage = "Usage is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid usage")]
    public int UsageId { set; get; }

    [Required(ErrorMessage = "Service is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid service")]
    public int ServiceId { set; get; }

    [Required(ErrorMessage = "Sales type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid sales type")]
    public int SalesTypeId { set; get; }

    [Required(ErrorMessage = "Property type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid property type")]
    public int PropertyTypeId { set; get; }

    public ClientBudget ClientBudget { set; get; } = new();
    [Required(ErrorMessage = "Agent is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Agent")]
    public int AgentId { get; set; }
    public long LeadTicketId { get; set; }
}