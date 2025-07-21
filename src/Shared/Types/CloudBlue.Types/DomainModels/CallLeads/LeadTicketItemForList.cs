using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class LeadTicketItemForList : IEntityBaseItem

{
    #region Client Data
    public bool IsPotential { get; set; }
    public string? WorkField { get; set; }

    public string? ClientName { get; set; }
    public bool IsVip { get; set; }


    public string? Gender { get; set; }
    public string? ClientPhone { get; set; }

    public string? ClientNameArabic { get; set; }
    public long ClientId { get; set; }

    public string? ClientTitle { get; set; }

    public string? ClientType { get; set; }

    public DateTime? ClientBirthDate { get; set; }

    public string? ClientCategory { get; set; }

    public string? ClientCompanyName { get; set; }

    public List<ClientPhoneItem> ClientContactDevices { get; set; } = new();

    public string? ClientOccupation { get; set; }

    #endregion

    #region Lead Ticket Data

    public int LastAgentFeedBackId { set; get; }
    public string? LastAgentFeedBackNote { get; set; }
    public string? LastAgentFeedBack { get; set; }




    public LeadTicketUsages UsageId { get; set; }
    public bool? IsFullLeadTicket { get; set; }

    public string? StatusFontColor { get; set; }
    public string? Usage { get; set; }



    public string? LeadSource { get; set; }

    public string? LeadTicketNote { get; set; }

    public string? Location { get; set; }

    public string? MarketingAgency { get; set; }



    public string? PropertyType { get; set; }


    public int CompanyId { get; set; }
    public string? LeadTicketStatus { get; set; }
    public string? SalesType { get; set; }
    public LeadTicketServices ServiceId { get; set; }
    public SalesTypes SalesTypeId { get; set; }

    public string? Service { get; set; }

    public LeadTicketStatuses LeadTicketStatusId { get; set; }


    public string? SalesTypeFontColor { get; set; }
    public object SalesTypeBadgeStyle { get; set; } = null!;
    public string? SalesTypeBackgroundColor { get; set; }
    public string? ServiceFontColor { get; set; }
    public string? ServiceBackgroundColor { get; set; }
    public object ServiceBadgeStyle { get; set; } = null!;

    public long ReferralId { get; set; }

    public int TcrStatusId { get; set; }


    public SystemPrivileges[] AllowedActions { get; set; } = [];
    public int TcrTypeId { get; set; }

    public bool WasOld { get; set; }

    public string? StatusBackgroundColor { get; set; }
    public object StatusBadgeStyle { get; set; } = null!;

    public string? UsageFontColor { get; set; }

    public string? UsageBackgroundColor { get; set; }
    public object UsageBadgeStyle { get; set; } = null!;

    public string? CurrencySymbol { get; set; }

    public string? BranchName { get; set; }

    public long CallId { get; set; }
    public string? FormName { get; set; }
    public string? CompanyName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreationDate { get; set; }
    public int ResignDateNumeric { get; set; }
    public bool IsApproved { set; get; }
    public DateTime? ResignDate { get; set; }

    public long Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Project { get; set; }

    public bool IsArchived { get; set; }

    public bool IsClosed { get; set; }

    public bool IsVoided { get; set; }

    public string? KnowSource { get; set; }

    public string? KnowSubSource { get; set; }






    public string? CorporateCompany { get; set; }

    public string? Currency { get; set; }

    public int BranchId { get; set; }

    public decimal BudgetFrom { get; set; }

    public decimal BudgetTo { get; set; }


    #endregion

    #region Agent and Activities Data

    public string? TopMostManagerName { get; set; }

    public bool ViewedByCurAgent { get; set; }

    public DateTime? AgentLastActivityDate { set; get; }

    public List<SystemEventItem> SystemEvents { get; set; } = new();

    public string? CurrentAgent { get; set; }
    public DateTime? SetInProgressDate { get; set; }

    public string? LastEventComment { get; set; }

    public DateTime? LastEventCreationDateTime { get; set; }

    public long LastEventId { get; set; }

    public string? LastEventProcess { get; set; }

    public string? AgentPosition { get; set; }
    public int CurrentAgentId { get; set; }


    public bool PendingAlreadyExistView { get; set; }


    public string? AgentLastEventComment { get; set; }
    public DateTime? CallBackDate { get; set; }

    public int HoursToGetInProgress { get; set; }

    public string? DirectManagerName { get; set; }

    public DateTime? ExtendedDate { get; set; }


    public DateTime? LastAssignedDate { get; set; }

    public string? AgentLastEventContactingType { get; set; }
    public long AgentLastEventId { get; set; }

    #endregion

    #region Stats  Data
    public bool ReassignedOnce { get; set; }

    public int ReassignCount { get; set; }

    public int ReassignedNewOnce { get; set; }

    public int SettingInProgressCount { get; set; }
    public int AlreadyExistCount { get; set; }

    /// <summary>
    /// ///////////////////
    /// </summary>

    public int UnQualifiedCount { get; set; }
    public int UnQualifiedDistinctCount { get; set; }

    public int QualifiedCount { get; set; }

    public int QualifiedDistinctCount { get; set; }

    public int NoAnswerCount { get; set; }

    public int NoAnswerDistinctCount { get; set; }
    public int CallLaterCount { get; set; }

    public int CallLaterDistinctCount { get; set; }



    public ActivityStatItem[] ActivityStats { get; set; } = [];

    public int RemindersDistinctCount { get; set; }
    public int RemindersCount { get; set; }




    public int ActiveRemindersCount { get; set; }




    public int ActiveCallLaterCount { get; set; }
    public string? OriginalProject { get; set; }
    public int[]? ManagersIdsArray { get; set; }

    #endregion









}