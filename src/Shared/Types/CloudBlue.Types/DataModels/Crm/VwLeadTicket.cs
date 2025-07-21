using System.Text.Json;

namespace CloudBlue.Domain.DataModels.Crm;

public class VwLeadTicket : BaseDataModel<long>
{
    public int RemindersDistinctCount { get; set; }
    public int RemindersCount { get; set; }
    public DateTime? LastAgentFeedBackDate { get; set; }



    public long LastAgentFeedBackDateNumeric { get; set; }

    public int[]? ManagersIdsArray { get; set; } = [];
    public int ActiveCallLaterCount { get; set; }
    public int ActiveRemindersCount { get; set; }
    public string? OriginalProject { get; set; }
    public int OriginalProjectId { get; set; }

    public string? ClientName { get; set; }
    public string? ClientNameLowered { get; set; }
    public string? TeleSalesAgentName { get; set; }

    public int TeleSalesAgentId { get; set; }
    public string? ClientNameArabic { get; set; }
    public int TopMostManagerId { get; set; }

    public string? TopMostManagerName { get; set; }

    public int DirectManagerId { get; set; }

    public string? DirectManagerName { get; set; }

    public JsonDocument? ContactDevicesJsonb { get; set; }

    public bool IsPotential { get; set; }

    public bool IsVip { get; set; }

    public string? BranchName { get; set; }

    public string? CompanyName { get; set; }

    public int BranchId { get; set; }

    public decimal BudgetFrom { get; set; }

    public decimal BudgetTo { get; set; }

    public DateTime? CallBackDate { get; set; }

    public long? CallBackDateNumeric { get; set; }

    public long CallId { get; set; }

    public int CompanyId { get; set; }

    public string? CreatedBy { get; set; }

    public int CreatedById { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreationDateNumeric { get; set; }

    public string? CurrentAgent { get; set; }

    public int CurrentAgentId { get; set; }

    public int DistrictId { get; set; }

    public DateTime? ExtendedDate { get; set; }

    public long? ExtendedDateNumeric { get; set; }

    public string? FormName { get; set; }

    public bool IsArchived { get; set; }

    public bool IsClosed { get; set; }

    public bool IsFullLeadTicket { get; set; }

    public bool IsVoided { get; set; }

    public DateTime? LastAssignedDate { get; set; }
    public DateTime? ClientBirthDate { get; set; }

    public int LastAssignedDateNumeric { get; set; }

    public string? LeadTicketNote { get; set; }

    public string? Location { get; set; }

    public int NeighborhoodId { get; set; }

    public bool PendingAlreadyExistView { get; set; }
    public int SalesTypeId { get; set; }

    public int KnowSourceExtraId { get; set; }

    public int KnowSourceId { get; set; }

    public int LeadSourceId { get; set; }

    public int UsageId { get; set; }

    public int PropertyTypeId { get; set; }

    public bool ViewedByCurAgent { get; set; }

    public string? ClientTitle { get; set; }

    public string? ClientType { get; set; }

    public string? KnowSource { get; set; }

    public string? KnowSubSource { get; set; }

    public string? LeadSource { get; set; }

    public string? PropertyType { get; set; }

    public string? SalesType { get; set; }

    public string? Usage { get; set; }

    public int ServiceId { get; set; }

    public int MarketingAgencyId { get; set; }

    public int CorporateCompanyId { get; set; }

    public int CurrencyId { get; set; }

    public int LeadTicketStatusId { get; set; }

    public string? Service { get; set; }

    public string? Currency { get; set; }
    public string? UsageFontColor { get; set; }

    public string? UsageBackgroundColor { get; set; }

    public string? CurrencySymbol { get; set; }

    public string? CorporateCompany { get; set; }

    public string? MarketingAgency { get; set; }

    public string? StatusFontColor { get; set; }

    public string? StatusBackgroundColor { get; set; }

    public string? LeadTicketStatus { get; set; }
    public string? SalesTypeFontColor { get; set; }
    public string? SalesTypeBackgroundColor { get; set; }
    public string? ServiceFontColor { get; set; }
    public string? ServiceBackgroundColor { get; set; }

    public long ClientId { get; set; }
    public long AgentLastEventCreationDateTimeNumeric { get; set; }

    public int LastAgentFeedBackId { set; get; }
    public string? LastAgentFeedBackNote { get; set; }
    public string? LastAgentFeedBack { get; set; }





    public long ReferralId { get; set; }

    public int TcrStatusId { get; set; }

    public int TcrTypeId { get; set; }

    public bool WasOld { get; set; }

    public int FirstOwnerId { get; set; }

    public int ReassignCount { get; set; }

    public int ReassignedNewOnce { get; set; }

    public bool ReassignedOnce { get; set; }

    public int SetInProgressDateNumeric { get; set; }
    public DateTime? SetInProgressDate { get; set; }

    public int AlreadyExistCount { get; set; }

    public JsonDocument? RecentEventsJsonb { get; set; }
    public string? ClientCategory { get; set; }
    public int ClientCategoryId { get; set; }
    public int CityId { get; set; }
    public int CountryId { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string? Project { get; set; }
    public int SettingInProgressCount { get; set; }
    public string? Gender { get; set; }
    public string? WorkField { get; set; }
    public string? ClientOccupation { get; set; }
    public string? ClientCompanyName { get; set; }

    public string? AgentLastEventComment { get; set; }

    public JsonDocument? ActivityStatsJsonb { get; set; }

    public int QualifiedDistinctCount { get; set; }

    public int UnQualifiedDistinctCount { get; set; }

    public int CallLaterDistinctCount { get; set; }

    public int NoAnswerDistinctCount { get; set; }

    public int QualifiedCount { get; set; }

    public int UnQualifiedCount { get; set; }

    public int CallLaterCount { get; set; }

    public int NoAnswerCount { get; set; }


    public int ResignDateNumeric { get; set; }
    public bool IsApproved { set; get; }
    public DateTime? ResignDate { get; set; }
    public DateTime? LastEventCreationDateTime { get; set; }

    public long LastEventCreationDateTimeNumeric { get; set; }

    public string? LastEventComment { get; set; }

    public int LastEventProcessId { get; set; }

    public long AgentLastEventId { get; set; }

    public long LastEventId { get; set; }

    public int HoursToGetInProgress { get; set; }

    public int AgentLastEventContactingTypeId { get; set; }

    public DateTime? AgentLastEventDateTime { get; set; }

    public long AgentLastEventDateTimeNumeric { get; set; }

    public int AgentLastEventTypeId { get; set; }

    public DateTime? AgentLastEventCreationDateTime { get; set; }
    public string? AgentPosition { get; set; }
}