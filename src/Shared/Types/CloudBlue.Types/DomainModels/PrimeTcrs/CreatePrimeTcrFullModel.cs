using System.Text.Json;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class CreatePrimeTcrFullModel
{
    public long ClientId { get; set; }
    public bool IsCorporate { get; set; }
    public decimal OutsideBrokerCommissionValue { set; get; }
    public decimal OutsideBrokerCommissionPercentage { set; get; }

    public int TeleSalesAgentId { get; set; }
    public string? TeleSalesAgentName { get; set; }
    public int ClosingChannelId { get; set; }
    public int ClosingChannelExtraId { get; set; }
    public int LastMarketingChannelId { get; set; }
    public int LastMarketingChannelExtraId { get; set; }
    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public int UsageId { get; set; }

    public string? FirstAgentName { get; set; }

    public string? ExtraManagerName { get; set; }

    public string? SecondAgentName { get; set; }

    public string? ThirdAgentName { get; set; }

    public int PrimeTcrStatusId { get; set; }

    public decimal CompanyCommissionValue { get; set; }

    public decimal CompanyCommissionPercentage { get; set; }

    public int CreatedById { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreationDateTime { get; set; }

    public int CreationDateNumeric { get; set; }

    public int PendingStageId { get; set; }

    public bool IsRegular { get; set; }

    public bool IsHalfCommission { get; set; }

    public string? FirstAgentTreeNames { get; set; }

    public JsonDocument? FirstAgentTreeJsonb { get; set; }

    public bool FirstAgentPromoted { get; set; }

    public int FirstAgentPositionId { get; set; }

    public int FirstAgentPositionAtHalfContractDateId { get; set; }

    public bool FirstAgentInResaleTeam { get; set; }

    public int FirstAgentPositionAtContractDateId { get; set; }

    public string? SecondAgentTreeNames { get; set; }

    public JsonDocument? SecondAgentTreeJsonb { get; set; }

    public bool SecondAgentPromoted { get; set; }

    public int SecondAgentPositionId { get; set; }

    public int SecondAgentPositionAtHalfContractDateId { get; set; }

    public bool SecondAgentInResaleTeam { get; set; }

    public int SecondAgentPositionAtContractDateId { get; set; }

    public string? ThirdAgentTreeNames { get; set; }

    public JsonDocument? ThirdAgentTreeJsonb { get; set; }

    public bool ThirdAgentPromoted { get; set; }

    public int ThirdAgentPositionId { get; set; }

    public int ThirdAgentPositionAtHalfContractDateId { get; set; }

    public bool ThirdAgentInResaleTeam { get; set; }

    public int ThirdAgentPositionAtContractDateId { get; set; }

    public int FirstReferralAgentId { get; set; }

    public string? FirstReferralAgentName { get; set; }

    public string? FirstReferralAgentTreeNames { get; set; }

    public JsonDocument? FirstReferralAgentTreeJsonb { get; set; }

    public int FirstReferralAgentPositionId { get; set; }

    public bool FirstReferralAgentInResaleTeam { get; set; }

    public bool FirstReferralAgentPromoted { get; set; }

    public long FirstReferralItemId { get; set; }

    public int SecondReferralAgentId { get; set; }

    public string? SecondReferralAgentName { get; set; }

    public string? SecondReferralAgentTreeNames { get; set; }

    public JsonDocument? SecondReferralAgentTreeJsonb { get; set; }

    public int SecondReferralAgentPositionId { get; set; }

    public bool SecondReferralAgentInResaleTeam { get; set; }

    public bool SecondReferralAgentPromoted { get; set; }

    public long SecondReferralItemId { get; set; }

    public bool IsReOpen { get; set; }

    public int SalesAccountingFeedBackId { get; set; }

    public bool IsCompanyCommissionCollected { get; set; }

    public DateTime? RecReserveDate { get; set; }

    public int RecReserveDateNumeric { get; set; }

    public DateTime? ConfirmedReservingDate { get; set; }

    public int ConfirmedReservingDateNumeric { get; set; }

    public DateTime? RecCloseDate { get; set; }

    public int RecCloseDateNumeric { get; set; }

    public string? LastFeedBack { get; set; }

    public DateTime? LastPostponeDate { get; set; }

    public int LastPostponeDateNumeric { get; set; }

    public bool IsResolved { get; set; }

    public bool IsDeleted { get; set; }

    public int VerificationStatusId { get; set; }

    public string? VerificationComment { get; set; }

    public decimal DueBalance { get; set; }

    public decimal TaxPercentage { get; set; }

    public decimal CollectedCommissionValue { get; set; }

    public DateTime? ConfirmedContractingDate { get; set; }

    public int ConfirmedContractingDateNumeric { get; set; }

    public DateTime? ConfirmedHalfContractingDate { get; set; }

    public int ConfirmedHalfContractingDateNumeric { get; set; }

    public bool IsHalfContracted { get; set; }

    public long CutOffId { get; set; }

    public DateTime? LastDeveloperReviewingDate { get; set; }

    public int LastDeveloperReviewingDateNumeric { get; set; }

    public bool HalfCommissionPaid { get; set; }

    public long HalfCommissionCutOffId { get; set; }

    public string? LastDeveloperFeedBack { get; set; }

    public DateTime? LastDeveloperFeedBackDate { get; set; }

    public int LastDeveloperFeedBackDateNumeric { get; set; }

    public DateTime? LastResolveDate { get; set; }

    public int LastResolveDateNumeric { get; set; }

    public DateTime? LastConflictDate { get; set; }

    public int LastConflictDateNumeric { get; set; }

    public bool Invoiced { get; set; }

    public int LastReopenDateNumeric { get; set; }

    public DateTime? LastReopenDate { get; set; }

    public bool ForceHalfDeal { get; set; }

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
    public string? DeveloperName { get; set; }
    public string? ProjectName { get; set; }
    public long PrimeTcrId { get; set; }
    public string? OutsideBrokerName { get; set; }

    public long LeadTicketId { get; set; }

    public int DeveloperId { get; set; }

    public int HasDocumentId { get; set; }

    public int ProjectId { get; set; }
    public string? Phase { get; set; }

    public int PropertyTypeId { get; set; }

    public decimal SalesVolume { get; set; }

    public string UnitNumber { get; set; } = null!;

    public decimal LandArea { get; set; }
    public decimal BuildUpArea { get; set; }

    public DateTime? ContractExpectedDate { get; set; }
    public string? Remarks { get; set; }

    public int OutsideBrokerId { get; set; }

    public int FirstAgentId { get; set; }

    public decimal FirstAgentSharePercentage { get; set; }

    public int ExtraManagerId { get; set; }

    public int SecondAgentId { get; set; }

    public decimal SecondAgentSharePercentage { get; set; }

    public int ThirdAgentId { get; set; }

    public decimal ThirdAgentSharePercentage { get; set; }

    public DateTime? DocumentDate { get; set; }

    public int DocumentTypeId { get; set; }

    public bool HaveDocument { get; set; }
    public bool IsRecContracted { get; set; }
    public int[] AgentsIdsArray { get; set; } = [];
    public int[] ManagersIdsArray { get; set; } = [];
    public string? ClientName { get; set; }
}