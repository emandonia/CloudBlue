using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class CallCreateModel : ITextInfos
{
    public ClientInfoModel ClientInfo { get; set; } = new();
    public LeadSourceInfoModel LeadSourceInfo { get; set; } = new();
    public long ClientId { get; set; }
    [Required(ErrorMessage = "Call type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid call type")]
    public int CallTypeId { get; set; }

    public int BranchId { get; set; }
    [Required(ErrorMessage = "Company is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid company")]
    public int CompanyId { get; set; }

    public int CallStatusId { get; set; }

    public LocationModel Location { set; get; } = new();
    public LeadTicketPartialModel LeadTicketModel { get; set; } = new();
    [Required(ErrorMessage = "Call comment is required")]
    public string? CallComment { get; set; }

    public string? Duration { get; set; }
    public int DurationInSeconds { get; set; }
    public string? LocationStr { get; set; }

    public bool ClientExists { get; set; }
    public string? StatusReason { get; set; }
    public long CallId { get; set; }
    public string? AgentName { get; set; }
    public string? BranchName { get; set; }
    public string? CompanyName { get; set; }
    public string? CurrentUserName { get; set; }
}