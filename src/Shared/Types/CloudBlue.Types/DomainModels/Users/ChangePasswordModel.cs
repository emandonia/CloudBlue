using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.Users;
public class ChangePasswordModel
{
    [Required(ErrorMessage = "Old Password is required")]
    public string OldPassword { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(12, ErrorMessage = "Invalid password, should be 8-12 characters")]
    [MinLength(8, ErrorMessage = "Invalid password, should be 8-12 characters")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Confirm Password is required")]
    [MaxLength(12, ErrorMessage = "Invalid password, should be 8-12 characters")]
    [MinLength(8, ErrorMessage = "Invalid password, should be 8-12 characters")]
    public string ConfirmPassword { get; set; } = null!;

}
