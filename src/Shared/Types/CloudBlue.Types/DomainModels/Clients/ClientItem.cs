namespace CloudBlue.Domain.DomainModels.Clients;

public class ClientItem
{
    public long Id { get; set; }

    public string? ClientName { get; set; } = null!;

    public string? ClientNameArabic { get; set; }

    public int ClientTypeId { get; set; }

    public string? ClientCompanyName { get; set; }

    public int ClientTitleId { get; set; }

    public DateOnly? ClientBirthDate { get; set; }

    public string? ClientOccupation { get; set; }

    public int ClientWorkFieldId { get; set; }
    public string? Email { set; get; }

    public List<ClientPhoneItem> ClientContactDevices { get; set; } = new();
    public int GenderId { get; set; }
}