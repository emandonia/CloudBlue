using CloudBlue.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels;

public class ClientPhoneModel
{
    public long Id { get; set; }

    public string? CountryCode { get; set; } = "0020";
    [Required(ErrorMessage = "Country is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid country")]
    public int CountryId { get; set; } = 73;
    [Required(ErrorMessage = "Area code is required")]
    [RegularExpression(@"^-?\d+$")]
    [MaxLength(4, ErrorMessage = "Invalid area code")]
    [MinLength(2, ErrorMessage = "Invalid area code, minimum two digits")]
    public string? AreaCode { get; set; }
    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^-?\d+$")]

    [MaxLength(8, ErrorMessage = "Invalid phone")]
    [MinLength(6, ErrorMessage = "Invalid phone, minimum six digits")]

    public string? Phone { get; set; }
    public string DeviceInfo { get; set; } = null!;
    public DeviceTypes DeviceType { get; set; } = DeviceTypes.Mobile;
    [Required(ErrorMessage = "Device type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid device type")]

    public int DeviceTypeId { get; set; } = 1;
    public bool IsDefault { get; set; }
    public bool IsNew { get; set; }
    public bool CanBeRemoved { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]

    public string? Email { get; set; }
}