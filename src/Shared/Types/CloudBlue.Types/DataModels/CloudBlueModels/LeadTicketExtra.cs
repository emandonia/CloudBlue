using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LeadTicketExtra
{
    public long Id { get; set; }

    public bool IsOld { get; set; }

    public int TcrStatusId { get; set; }

    public int TcrTypeId { get; set; }

    public int ExtendedStatusId { get; set; }

    public bool IsClosed { get; set; }

    public bool ShowDummyToAgent { get; set; }

    public string? RejectReason { get; set; }

    public DateTime? RejectDate { get; set; }

    public long LastAssignedDateNumeric { get; set; }

    public DateTime? LastAssignedDate { get; set; }

    public long LastDeactivatedDateNumeric { get; set; }

    public DateTime? LastDeactivatedDate { get; set; }

    public long LastEventId { get; set; }

    public long? LastVoidedDateNumeric { get; set; }

    public DateTime? LastVoidedDate { get; set; }

    public long PrevLastEventId { get; set; }

    public long AgentLastEventId { get; set; }

    public bool IsCorporate { get; set; }

    public bool IsVip { get; set; }

    public bool IsOptedOut { get; set; }

    public bool IsPotential { get; set; }

    public long TeleSalesAgentId { get; set; }

    public int OldStatusId { get; set; }

    public bool IsReserved { get; set; }

    public long OldAgentId { get; set; }

    public bool WasOld { get; set; }

    public long LastEventDateNumeric { get; set; }

    public long SettingOldLogId { get; set; }

    public int AlreadyExistCount { get; set; }

    public bool PendingAlreadyExistView { get; set; }

    public int VoidReasonId { get; set; }

    public long SetInProgressDateNumeric { get; set; }

    public long FirstAgentEventAfterAssign { get; set; }

    public int DaysToGetInProgress { get; set; }

    public long AssociatedLeadId { get; set; }

    public long ExpRegId { get; set; }

    public bool ReassignedOnce { get; set; }

    public int ReassignedNewOnce { get; set; }

    public int ReassignCount { get; set; }

    public long CampaignOwnerId { get; set; }

    public long WebLeadId { get; set; }

    public bool ApplyCampaignOwnerShipRules { get; set; }

    public long FirstOwnerId { get; set; }

    public bool ApplyTwentyFourHoursRules { get; set; }

    public DateTime? LastRevivalDate { get; set; }

    public long LastRevivalDateNumeric { get; set; }

    public long RevivedById { get; set; }

    public long RevivedByAgentId { get; set; }

    public long OldLeadRefId { get; set; }

    public long CollectiveCampaignId { get; set; }

    public string? AgencyAbbrev { get; set; }

    public long ProjectCampaignId { get; set; }

    public long AgentLastEventCreationDateTimeNumeric { get; set; }

    public int LastEventEventProcessId { get; set; }

    public int EventTypeId { get; set; }

    public long EventDateTimeNumeric { get; set; }

    public int ContactingDeviceId { get; set; }

    public DateTime? EventDateTime { get; set; }

    public string? EventComment { get; set; }

    public long LastConversionEventId { get; set; }

    public DateTime? CallBackDate { get; set; }

    public long CallBackDateNumeric { get; set; }

    public DateTime? ExtendedDate { get; set; }

    public long ExtendedDateNumeric { get; set; }

    public bool WrongNumberAction { get; set; }

    public int CorporateCompanyId { get; set; }

    public bool IsPureOld { get; set; }

    public long DuplicateLeadId { get; set; }

    public bool IsDuplicated { get; set; }

    public string? CurrentAgentManagersTree { get; set; }

    public bool ClientNameUpdated { get; set; }

    public int ClientCategoryId { get; set; }

    public long PreAgLastEvId { get; set; }

    public long CorrectAgLastEvId { get; set; }

    public long LasteId { get; set; }

    public int PreAgLastEvTypeId { get; set; }

    public int PreAgLastEvProcId { get; set; }

    public virtual LeadTicket IdNavigation { get; set; } = null!;
}
