using System.Text.Json;

namespace CloudBlue.Domain.DataModels.Crm;

public class Client : BaseDataModel<long>
{
    public string? ClientName { get; set; }
    public string? ClientNameLowered { get; set; }

    public string? ClientNameArabic { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreationDateNumeric { get; set; }

    public int CreatedById { get; set; }

    public DateTime? LastEditDate { get; set; }

    public int LastEditDateNumeric { get; set; }

    public int ClientTypeId { get; set; }

    public string? ClientCompanyName { get; set; }

    public int CompanyId { get; set; }

    public string? Notes { get; set; }

    public int ClientTitleId { get; set; }

    public DateOnly? ClientBirthDate { get; set; }

    public string? ClientOccupation { get; set; }

    public int ClientOccupationFieldId { get; set; }

    public long WebLeadId { get; set; }

    public int LastEditorId { get; set; }

    public bool IsPotential { get; set; }

    public bool IsOptedOut { get; set; }

    public bool IsVip { get; set; }

    public int ClientCategoryId { get; set; }
    public ICollection<ClientContactDevice> ContactDevices { get; set; } = new List<ClientContactDevice>();
    public JsonDocument? ContactDevicesJsonb { get; set; }

    public int GenderId { get; set; }
}