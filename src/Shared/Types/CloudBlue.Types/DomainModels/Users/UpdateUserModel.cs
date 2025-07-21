using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.Users;

public class UpdateUserModel
{
    [Required(ErrorMessage = "Item is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid Item")]

    public int SelectedItemId { set; get; }
    [Required(ErrorMessage = "Date is required")]

    public DateTime? PromotionDate { set; get; }
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(12, ErrorMessage = "Invalid password, should be 8-12 characters")]
    [MinLength(8, ErrorMessage = "Invalid password, should be 8-12 characters")]
    public string Password { set; get; } = null!;
    public ClientPhoneModel UserPhone { set; get; } = new();

    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(50, ErrorMessage = "Max length 50 characters")]
    [MinLength(8, ErrorMessage = "Min length 8 characters")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(50, ErrorMessage = "Max length 50 characters")]
    [MinLength(8, ErrorMessage = "Min length 8 characters")]
    public string UserName { get; set; } = null!;
}