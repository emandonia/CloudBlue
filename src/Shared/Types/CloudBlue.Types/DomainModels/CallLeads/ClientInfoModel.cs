using CloudBlue.Domain.NewFolder;
using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.CallLeads;

public class ClientInfoModel
{
    [Required(ErrorMessage = "Client type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid client type")]
    public int ClientTypeId { set; get; }

    [Required(ErrorMessage = "Client title is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid client title")]
    public int ClientTitleId { set; get; }

    [Required(ErrorMessage = "Client name is required")]
    public string ClientName { set; get; } = null!;

    public string? ClientArabicName { set; get; }
    public string? ClientCompanyName { set; get; }
    public string? Occupation { set; get; }
    public int WorkFieldId { set; get; }
    [DateValidatorAttribute(18)]
    public DateOnly? BirthDate { set; get; }

    public string? Email { set; get; }
    public int GenderId { get; set; }
    public int ClientCategoryId { get; set; }
    public List<ClientPhoneModel> ClientContactDevices { get; set; } = new();
    public long ClientId { get; set; }
}