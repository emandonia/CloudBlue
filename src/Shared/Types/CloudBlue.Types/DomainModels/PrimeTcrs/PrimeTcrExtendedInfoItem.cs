using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class PrimeTcrExtendedInfoItem : PrimeTcrItemForList
{
    public string? Remarks { get; set; }

    public int OutsideBrokerId { get; set; }

    public decimal CompanyCommissionValue { get; set; }

    public decimal CompanyCommissionPercentage { get; set; }

    public bool IsRegular { get; set; }


    public decimal FirstAgentSharePercentage { get; set; }

    public string? FirstAgentTreeNames { get; set; }

    public bool FirstAgentInResaleTeam { get; set; }

    public string? SecondAgentName { get; set; }

    public decimal SecondAgentSharePercentage { get; set; }

    public string? SecondAgentTreeNames { get; set; }

    public bool SecondAgentInResaleTeam { get; set; }

    public string? ThirdAgentName { get; set; }

    public decimal ThirdAgentSharePercentage { get; set; }

    public string? ThirdAgentTreeNames { get; set; }

    public bool ThirdAgentInResaleTeam { get; set; }

    public string? FirstReferralAgentName { get; set; }

    public string? FirstReferralAgentTreeNames { get; set; }

    public bool FirstReferralAgentInResaleTeam { get; set; }

    public string? SecondReferralAgentName { get; set; }

    public string? SecondReferralAgentTreeNames { get; set; }

    public bool SecondReferralAgentInResaleTeam { get; set; }


    public bool IsDeleted { get; set; }


    public decimal TaxPercentage { get; set; }

    public decimal CollectedCommissionValue { get; set; }


    public string? Phase { get; set; }


    public decimal ForceAchievementPercentage { get; set; }

    public bool ForceFlatRateCommission { get; set; }

    public decimal ForceCommissionPercentage { get; set; }

    public bool SkipIncentive { get; set; }

    public bool RestrictTargetCommission { get; set; }

    public bool TcrSelection { get; set; }

    public string? ResignedRuleSkippedIds { get; set; }

    public bool SkipHalfCommissionRules { get; set; }

    public decimal ForcedAgentIncentiveValue { get; set; }

    public bool IgnoreDebitedIncentive { get; set; }

    public bool IgnoreDebitedCommission { get; set; }

    public string? FreezeCommissionManagersIds { get; set; }

    public string? FreezeCommissionAgentsIds { get; set; }

    public decimal ForcedScaledCommissionPercentage { get; set; }

    public decimal FirstAgentForcedCommissioningPercentage { get; set; }

    public decimal SecondAgentForcedCommissioningPercentage { get; set; }

    public decimal ThirdAgentForcedCommissioningPercentage { get; set; }

    public int PropertyTypeId { get; set; }

    public PrimeTcrStatuses PrimeTcrStatus { get; set; }

    public int LeadTicketCompanyId { get; set; }

    public int LeadTicketBranchId { get; set; }

    public int LeadTicketCreationDateNumeric { get; set; }

    public int ClosingChannelExtraId { get; set; }

    public int LastMarketingChannelId { get; set; }

    public int LastMarketingChannelExtraId { get; set; }
    public int ProjectId { get; set; }
}