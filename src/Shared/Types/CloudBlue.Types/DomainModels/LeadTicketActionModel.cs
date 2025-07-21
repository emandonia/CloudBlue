using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;

public class LeadTicketActionModel : ITextInfos
{
    public SystemPrivileges Action { get; set; }
    public List<long> ItemsIds { get; set; } = new();
    [Required(ErrorMessage = "Company is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid company")]

    public int CompanyId { get; set; }
    [Required(ErrorMessage = "Branch is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Branch")]

    public int BranchId { get; set; }

    [Required(ErrorMessage = "Agent is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Agent")]

    public int AgentId { get; set; }
    [Required(ErrorMessage = "Comment is required")]

    public string? Comment { get; set; }

    [Required(ErrorMessage = "Type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Type")]

    public int ContactingTypeId { get; set; }

    [Required(ErrorMessage = "Reason is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Reason")]

    public int ReasonId { get; set; }
    [Required(ErrorMessage = "Date is required")]

    public DateTime? ReminderDate { get; set; }

    public string? Reason { get; set; }
    public bool SkipTemplate { get; set; }
    public bool IsReminder { get; set; }
    public int CurrentAgentId { get; set; }
    public string? AgentName { get; set; }
    public string? BranchName { get; set; }
    public string? CompanyName { get; set; }
    public string? CurrentUserName { get; set; }
}