namespace CloudBlue.Domain.DataModels.Crm;

public class LeadTicket : BaseDataModel<long>
{
    public virtual LeadTicketExtension LeadTicketExtension { get; set; } = null!;

    public long ClientId { get; set; }
    public int LastAgentFeedBackId { set; get; }
    public string? LastAgentFeedBackNote { get; set; }

    public int BranchId { get; set; }
    public int RemindersDistinctCount { get; set; }
    public int RemindersCount { get; set; }
    public int ActiveCallLaterCount { get; set; }
    public int ActiveRemindersCount { get; set; }

    public int CompanyId { get; set; }

    public int LeadTicketStatusId { get; set; }

    public string? LeadTicketNote { get; set; }

    public int UsageId { get; set; }

    public int SalesTypeId { get; set; }

    public int PropertyTypeId { get; set; }

    public int DistrictId { get; set; }

    public int NeighborhoodId { get; set; }

    public decimal BudgetFrom { get; set; }

    public decimal BudgetTo { get; set; }

    public int CurrencyId { get; set; }

    public int ServiceId { get; set; }

    public int CurrentAgentId { get; set; }

    public bool IsClosed { get; set; }

    public int CreatedById { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime? LastAgentFeedBackDate { get; set; }



    public long LastAgentFeedBackDateNumeric { get; set; }
    public int CreationDateNumeric { get; set; }

    public int LeadSourceId { get; set; }

    public int KnowSourceId { get; set; }

    public long CallId { get; set; }

    public int KnowSourceExtraId { get; set; }

    public bool IsArchived { get; set; }

    public bool IsFullLeadTicket { get; set; }

    public long ReferralId { get; set; }

    public bool IsOld { get; set; }

    public bool PendingAlreadyExistView { get; set; }

    public int HoursToGetInProgress { get; set; }

    public int TcrStatusId { get; set; }

    public int TcrTypeId { get; set; }

    public bool ApplyCampaignOwnerShipRules { get; set; }

    public bool ApplyTwentyFourHoursRules { get; set; }

    public int CampaignOwnerId { get; set; }

    public long CollectiveCampaignId { get; set; }

    public string? AgencyAbbrev { get; set; }

    public long ProjectCampaignId { get; set; }

    public bool IsVoided { get; set; }

    public string? VoidingReason { get; set; }

    public DateTime? DateVoided { get; set; }

    public string? FormName { get; set; }

    public string? AdLink { get; set; }

    public string? Location { get; set; }

    public int CorporateCompanyId { get; set; }

    public int LastAssignedDateNumeric { get; set; }

    public DateTime? LastAssignedDate { get; set; }

    public long LastEventId { get; set; }

    public long AgentLastEventId { get; set; }

    public long AgentLastEventCreationDateTimeNumeric { get; set; }

    public int LastEventProcessId { get; set; }

    public string? LastEventComment { get; set; }

    public DateTime? CallBackDate { get; set; }

    public long CallBackDateNumeric { get; set; }

    public DateTime? ExtendedDate { get; set; }

    public long ExtendedDateNumeric { get; set; }

    public bool WrongNumberAction { get; set; }

    public bool IsDuplicated { get; set; }

    public bool IsVip { get; set; }

    public bool IsOptedOut { get; set; }

    public bool IsPotential { get; set; }

    public bool ViewedByCurAgent { get; set; }

    public long AgentFirstEventIdAfterAssign { get; set; }


    public int ProspectRequestStatusId { get; set; }

    public int MarketingAgencyId { get; set; }

    public string? RecentEventsJsonb { get; set; }

    public int AlreadyExistCount { get; set; }

    public int SetInProgressDateNumeric { get; set; }

    public bool ReassignedOnce { get; set; }

    public int ReassignedNewOnce { get; set; }

    public int ReassignCount { get; set; }

    public int FirstOwnerId { get; set; }

    public bool WasOld { get; set; }

    public long WebLeadId { get; set; }

    public int SettingInProgressCount { get; set; }

    public long LastEventCreationDateTimeNumeric { get; set; }

    public DateTime? LastEventCreationDateTime { get; set; }

    public int AgentLastEventTypeId { get; set; }

    public long AgentLastEventDateTimeNumeric { get; set; }

    public DateTime? AgentLastEventDateTime { get; set; }

    public int NoAnswerCount { get; set; }

    public int CallLaterCount { get; set; }

    public int UnQualifiedCount { get; set; }

    public int QualifiedCount { get; set; }

    public int NoAnswerDistinctCount { get; set; }

    public int CallLaterDistinctCount { get; set; }

    public int UnQualifiedDistinctCount { get; set; }

    public int QualifiedDistinctCount { get; set; }

    public string? ActivityStatsJsonb { get; set; }

    public int AgentLastEventContactingTypeId { get; set; }

    public string? AgentLastEventComment { get; set; }

    public DateTime? AgentLastEventCreationDateTime { get; set; }
    public int VoidReasonId { get; set; }
    public int RejectReasonId { get; set; }
    public DateTime? SetInProgressDate { get; set; }
    public string? OriginalProject { get; set; }
    public int OriginalProjectId { get; set; }
}