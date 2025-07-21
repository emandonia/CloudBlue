using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class PrimeTcrItemForList
{
    public int[] AgentsIds { get; set; } = [];

    public int[] AgentsIdsArray { get; set; } = [];

    public SystemPrivileges[] AllowedActions { get; set; } = [];

    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public string? ClientName { get; set; }

    public string? ClosingChannel { get; set; }

    public string? ClosingSubChannel { get; set; }

    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public DateTime? ConfirmedContractingDate { get; set; }

    public DateTime? ConfirmedHalfContractingDate { get; set; }

    public DateTime? ConfirmedReservingDate { get; set; }

    public DateTime? ContractExpectedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreationDateTime { get; set; }

    public string? DeveloperName { get; set; }

    public DateTime? DocumentDate { get; set; }

    public int DocumentTypeId { get; set; }
    public bool ConvertedFromReferral { get; set; }
    public string? DocumentType { get; set; }
    public string? LeadProject { get; set; }
    public string? LeadDistrict { get; set; }

    public decimal DueBalance { get; set; }

    public string? ExtraManagerName { get; set; }

    public string? FirstAgentName { get; set; }

    public bool ForceHalfDeal { get; set; }

    public bool HalfCommissionPaid { get; set; }

    public bool HaveDocument { get; set; }

    public long Id { get; set; }

    public bool Invoiced { get; set; }

    public bool IsCompanyCommissionCollected { get; set; }

    public bool IsCorporate { get; set; }

    public bool IsHalfCommission { get; set; }

    public bool IsHalfContracted { get; set; }

    public bool IsReOpen { get; set; }

    public bool IsResolved { get; set; }

    public DateTime? LastConflictDate { get; set; }

    public string? LastDeveloperFeedBack { get; set; }

    public DateTime? LastDeveloperFeedBackDate { get; set; }

    public DateTime? LastDeveloperReviewingDate { get; set; }

    public string? LastFeedBack { get; set; }

    public string? LastMarketingChannel { get; set; }

    public string? LastMarketingSubChannel { get; set; }

    public DateTime? LastPostponeDate { get; set; }

    public DateTime? LastReopenDate { get; set; }

    public DateTime? LastResolveDate { get; set; }

    public string? LeadSource { get; set; }

    public DateTime? LeadTicketCreationDate { get; set; }

    public long LeadTicketId { get; set; }

    public int[] ManagersIds { get; set; } = [];

    public int[] ManagersIdsArray { get; set; } = [];

    public string? MarketingAgency { get; set; }

    public string? OutsideBrokerName { get; set; }

    public string? PrimeTcrStatusBackgroundColor { get; set; }

    public int PrimeTcrStatusId { get; set; }

    public string? PrimeTcrStatusName { get; set; }

    public string? ProjectName { get; set; }

    public string? PropertyType { get; set; }

    public DateTime? RecCloseDate { get; set; }

    public DateTime? RecReserveDate { get; set; }

    public decimal SalesVolume { get; set; }
    public decimal BuildUpArea { get; set; }

    public object StatusBadgeStyle { get; set; } = null!;

    public List<SystemEventItem> SystemEvents { get; set; } = new();

    public string? UnitNumber { get; set; }

    public string? Usage { get; set; }

    public int UsageId { get; set; }

    public int VerificationStatusId { get; set; }
    public string? FirstAgent { get; set; }
    public string? FirstAgentPosition { get; set; }
    public string? FirstAgentManagerOne { get; set; }
    public string? FirstAgentManagerTwo { get; set; }
    public string? FirstAgentManagerThree { get; set; }
    public string? FirstAgentManagerFour { get; set; }
    public string? FirstAgentManagerFive { get; set; }
    public string? LastReopenReason { get; set; }
    public int FirstAgentId { get; set; }
    public decimal FirstAgentPercentage { get; set; }

    public string? SecondAgent { get; set; }
    public string? SecondAgentPosition { get; set; }
    public string? SecondAgentManagerOne { get; set; }
    public string? SecondAgentManagerTwo { get; set; }
    public string? SecondAgentManagerThree { get; set; }
    public string? SecondAgentManagerFour { get; set; }
    public string? SecondAgentManagerFive { get; set; }
    public int SecondAgentId { get; set; }
    public decimal SecondAgentPercentage { get; set; }

    public string? ThirdAgent { get; set; }
    public string? ThirdAgentPosition { get; set; }
    public string? ThirdAgentManagerOne { get; set; }
    public string? ThirdAgentManagerTwo { get; set; }
    public string? ThirdAgentManagerThree { get; set; }
    public string? ThirdAgentManagerFour { get; set; }
    public string? ThirdAgentManagerFive { get; set; }
    public int ThirdAgentId { get; set; }
    public decimal ThirdAgentPercentage { get; set; }
    public string? FirstAgentTreeNames { get; set; }
    public string? ThirdAgentTreeNames { get; set; }
    public string? SecondAgentTreeNames { get; set; }
}