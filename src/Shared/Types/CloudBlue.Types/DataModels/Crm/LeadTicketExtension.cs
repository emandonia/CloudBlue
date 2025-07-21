using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DataModels.Crm;

public class LeadTicketExtension
{
    public virtual LeadTicket LeadTicket { get; set; } = null!;

    [Key]
    public long LeadTicketId { get; set; }
    public long VoidingWebLeadId { get; set; }
    public string? RejectReason { get; set; }

    public DateTime? RejectDate { get; set; }

    public int LastDeactivatedDateNumeric { get; set; }

    public DateTime? LastDeactivatedDate { get; set; }

    public int LastVoidedDateNumeric { get; set; }

    public DateTime? LastVoidedDate { get; set; }

    public bool ConvertedFromCall { get; set; }

    public bool ConvertedFromReferral { get; set; }

    public bool ConvertedFromDummy { get; set; }

    public int ExtendedStatusId { get; set; }

    public int TeleSalesAgentId { get; set; }

    public int OldStatusId { get; set; }

    public long AssociatedLeadId { get; set; }

    public long ExpRegId { get; set; }

    public bool IsCorporate { get; set; }

    public bool IsReserved { get; set; }

    public bool IsPureOld { get; set; }

    public long VoidingLeadTicketId { get; set; }

    public long OldLeadRefId { get; set; }

    public int SalesForceLeadId { get; set; }

    public bool ShowDummyToAgent { get; set; }

    public int OldAgentId { get; set; }

    public long SettingOldLogId { get; set; }

    public DateTime? LastRevivalDate { get; set; }

    public int LastRevivalDateNumeric { get; set; }

    public int RevivedById { get; set; }

    public int RevivedByAgentId { get; set; }

    public long LastConversionEventId { get; set; }

    public long DuplicateLeadId { get; set; }

    public string? CurrentAgentManagersTree { get; set; }

    public bool ClientNameUpdated { get; set; }
}