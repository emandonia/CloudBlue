using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class LeadTicket
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public int BranchId { get; set; }

    public int CompanyId { get; set; }

    public int LeadTicketStatusId { get; set; }

    public string? LeadTicketNote { get; set; }

    public int LineofBusinessId { get; set; }

    public int PropertyTypeId { get; set; }

    public int DistrictId { get; set; }

    public int NeighborhoodId { get; set; }

    public decimal BudgetFrom { get; set; }

    public decimal BudgetTo { get; set; }

    public int CurrencyId { get; set; }

    public int ServiceId { get; set; }

    public string? ServiceOther { get; set; }

    public long CurrentAgentId { get; set; }

    public string? LeadTicketPaperSerial { get; set; }

    public long CreatorId { get; set; }

    public DateTime CreationDate { get; set; }

    public long CreationDateNumeric { get; set; }

    public bool ViewedByCurAgent { get; set; }

    public bool ConvertedFromCall { get; set; }

    public int SourceId { get; set; }

    public string? SourceExtra { get; set; }

    public int KnowSourceId { get; set; }

    public string? KnowSourceExtra { get; set; }

    public DateTime? KnowDate { get; set; }

    public long CallId { get; set; }

    public string? OtherNeighbouhood { get; set; }

    public int KnowSourceExtraId { get; set; }

    public bool? IsArchived { get; set; }

    public bool ConvertedFromReferral { get; set; }

    public bool IsFullLeadTicket { get; set; }

    public long ReferralId { get; set; }

    public bool ConvertedFromDummy { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual LeadTicketExtra? LeadTicketExtra { get; set; }
}
