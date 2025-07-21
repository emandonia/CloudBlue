using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadTicketCreateModel : LeadTicketPartialModel, ITextInfos
{
    public long ClientId { get; set; }
    [Required(ErrorMessage = "Company is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid company")]
    public int CompanyId { get; set; }

    public int BranchId { get; set; }
    public LeadSourceInfoModel LeadSourceInfo { get; set; } = new();
    public ClientInfoModel ClientInfo { get; set; } = new();
    public long CallId { get; set; }
    public long ReferralId { get; set; }
    //[Required(ErrorMessage = "Comment is required")]
    public string? Comment { get; set; }

    public LocationModel Location { set; get; } = new();
    public string? LocationStr { get; set; }

    public int LeadTicketStatusId { get; set; }
    public bool IsFullLeadTicket { get; set; }
    public bool PendingAlreadyExistView { get; set; }
    public bool ApplyCampaignOwnerShipRules { get; set; }
    public bool ApplyTwentyFourHoursRules { get; set; }
    public DateTime? LastAssignedDate { get; set; }
    public int CorporateCompanyId { get; set; }
    public bool ClientExists { get; set; }
    [Required]
    [Range(1, 2)]
    public int IsCorporate { get; set; } = 2;

    public string? AgentName { get; set; }
    public string? BranchName { get; set; }
    public string? CompanyName { get; set; }
    public string? CurrentUserName { get; set; }
}