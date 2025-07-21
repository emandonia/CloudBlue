using CloudBlue.Domain.GenericTypes;
using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class TcrConfigsItem
{
    //1st row
    public bool IsRegular { get; set; }
    public bool IsHalfCommission { get; set; }
    [Range(0.0, 100, ErrorMessage = "Invalid percentage")]
    public decimal TaxPercentage { get; set; }
    [Range(0, 100, ErrorMessage = "Invalid percentage")]
    public decimal CompanyCommissionPercentage { get; set; }

    //second row


    [Range(0, 100, ErrorMessage = "Invalid percentage")]
    public decimal ForceAchievementPercentage { get; set; }
    [Range(0, 100, ErrorMessage = "Invalid percentage")]
    public decimal ForceCommissionPercentage { get; set; }
    public decimal ForcedAgentIncentiveValue { get; set; }
    [Range(0, 100, ErrorMessage = "Invalid percentage")]
    public decimal ForcedScaledCommissionPercentage { get; set; }

    //third row
    public bool SkipIncentive { get; set; }
    public bool IgnoreDebitedIncentive { get; set; }
    public bool IgnoreDebitedCommission { get; set; }
    public bool SkipHalfCommissionRules { get; set; }

    //fourth row
    public bool RestrictTargetCommission { get; set; }
    public bool TcrSelection { get; set; }

    public bool ForceHalfDeal { get; set; }
    public bool ForceFlatRateCommission { get; set; }

    //fifth row
    public string? ResignedRuleSkippedIdsStr { get; set; }
    public string? FreezeCommissionManagersIdsStr { get; set; }
    public string? FreezeCommissionAgentsIdsStr { get; set; }

    public IEnumerable<int> ResignedRuleSkippedIds { get; set; } = new List<int>();
    public IEnumerable<int> FreezeCommissionManagersIds { get; set; } = new List<int>();
    public IEnumerable<int> FreezeCommissionAgentsIds { get; set; } = new List<int>();

    public LookupItem<int>[] AgentsList { set; get; } = [];






}