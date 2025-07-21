using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.PrimeTcrs;

public class PrimeTcrFullItem
{
    public PrimeTcrAttachmentItem[] PrimeTcrAttachments { set; get; } = [];
    public string? DeveloperName { get; set; }
    public string? ProjectName { get; set; }
    public string? OutsideBrokerName { get; set; }
    public string? ClientName { get; set; }
    public string? CompanyName { get; set; }
    public string? BranchName { get; set; }
    public List<ClientPhoneItem> ClientContactDevices { get; set; } = new();
    public string? PropertyTypeName { get; set; }
    public string? Usage { get; set; }
    public string? PrimeTcrStatusName { get; set; }
    public string HalfCommission { get; set; } = "N/A";
    public string HalfConfirmed { get; set; } = "N/A";
    public string InvoicedStr { get; set; } = "N/A";
    public bool Invoiced { get; set; }
    public string? UnitNumber { get; set; }
    public int SalesVolume { get; set; }

    public decimal LandArea { get; set; }
    public decimal BuildUpArea { get; set; }

    public long LeadTicketId { get; set; }
    public DateTime? ContractExpectedDate { get; set; }
    public DateTime? DocumentDate { get; set; }
    public bool HasDocument { get; set; }
    public string? DocumentType { set; get; }
    public string? FirstAgentTreeNames { get; set; }

    public string? SecondAgentTreeNames { get; set; }

    public string? ThirdAgentTreeNames { get; set; }

    public string? FirstReferralAgentTreeNames { get; set; }

    public string? SecondReferralAgentName { get; set; }

    public string? SecondReferralAgentTreeNames { get; set; }
    public string? Remarks { get; set; }
    public decimal FirstAgentSharePercentage { set; get; }
    public decimal SecondAgentSharePercentage { set; get; }
    public decimal ThirdAgentSharePercentage { set; get; }

    public string? TeleSalesAgentName { get; set; }
    public DateTime LeadTicketCreationDate { get; set; }
    public DateTime CreationDateTime { get; set; }
    public long PrimeTcrId { get; set; }
    public bool IsCorporate { get; set; }
    public string? FirstReferralAgentName { get; set; }

    public List<SystemEventItem> SystemEvents { get; set; } = new();

    public string? FirstAgentName { get; set; }

    public string? ExtraManagerName { get; set; }

    public string? SecondAgentName { get; set; }

    public string? ThirdAgentName { get; set; }

    public DateTime? ConfirmedContractingDate { get; set; }
    public DateTime? ConfirmedHalfContractingDate { get; set; }

    public DateTime? RecReserveDate { get; set; }
    public DateTime? RecCloseDate { get; set; }

    public DateTime? LastDeveloperReviewingDate { get; set; }
    public DateTime? LastDeveloperFeedBackDate { get; set; }

    public DateTime? LastConflictDate { get; set; }
    public DateTime? LastReopenDate { get; set; }
    public DateTime? VerificationDate { get; set; }

    public DateTime? LastResolveDate { get; set; }
    public DateTime? LastPostponeDate { get; set; }

    public bool IsResolved { get; set; }
    public bool IsReOpen { get; set; }

    public decimal CompanyCommissionValue { get; set; }

    public decimal OutsideBrokerCommissionValue { set; get; }
    public decimal OutsideBrokerCommissionPercentage { set; get; }

    public decimal CollectedCommissionValue { get; set; }
    public bool IsDeleted { get; set; }
    public string? VerificationStatus { get; set; }
    public string? VerificationComment { get; set; }

    public string? LastDeveloperFeedBack { get; set; }


    public bool IsHalfContracted { get; set; }
    public decimal DueBalance { get; set; }
    public bool IsCompanyCommissionCollected { get; set; }
    public TcrConfigsItem TcrConfigs { set; get; } = new();
    public string? ClientPhones { get; set; }
    public int PrimeTcrStatusId { get; set; }
    public int VerificationStatusId { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public int[] ManagersIdsArray { get; set; } = [];
    public int[] AgentsIdsArray { get; set; } = [];
    public DateTime? ConfirmedReservingDate { get; set; }

    public bool HalfCommissionPaid { get; set; }
    public SystemPrivileges[] AllowedActions { get; set; } = [];
    public int FirstAgentId { get; set; }
    public int SecondAgentId { get; set; }
    public int ThirdAgentId { get; set; }
    public int UsageId { get; set; }
    public bool IsHalfCommission { get; set; }
    public bool ForceHalfDeal { get; set; }
    public int FirstReferralAgentId { get; set; }
    public int SecondReferralAgentId { get; set; }
}