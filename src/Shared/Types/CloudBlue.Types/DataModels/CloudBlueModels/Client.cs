using System;
using System.Collections.Generic;

namespace DataImporterLib.CloudBlueModels;

public partial class Client
{
    public long Id { get; set; }

    public string? ClientName { get; set; }

    public string? ClientNameAr { get; set; }

    public DateTime CreationDate { get; set; }

    public long CreationDateNumeric { get; set; }

    public long CreatorId { get; set; }

    public DateOnly? LastEditDate { get; set; }

    public long LastEditDateNumeric { get; set; }

    public long LastEditId { get; set; }

    public int ClientTypeId { get; set; }

    public string? ClientCompanyName { get; set; }

    public int CompanyId { get; set; }

    public string? Notes { get; set; }

    public int ClientTitleId { get; set; }

    public bool IsPotential { get; set; }

    public bool IsVip { get; set; }

    public bool IsOptedOut { get; set; }

    public DateOnly? ClientBirthDate { get; set; }

    public string? ClientOccupation { get; set; }

    public int ClientOccupationFieldId { get; set; }

    public long WebLeadId { get; set; }

    public virtual ClientAddress? ClientAddress { get; set; }

    public virtual ICollection<ClientContactDevice> ClientContactDevices { get; set; } = new List<ClientContactDevice>();

    public virtual ICollection<LeadTicket> LeadTickets { get; set; } = new List<LeadTicket>();
}
