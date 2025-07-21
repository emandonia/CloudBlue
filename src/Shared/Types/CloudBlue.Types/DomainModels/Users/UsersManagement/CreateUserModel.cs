using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.Users.UsersManagement;

public class CreateUserModel

{
    [Required(ErrorMessage = "This option is required")]
    [Range(1, 2, ErrorMessage = "Invalid value")]

    public int CanUserAccessPortal { get; set; }
    [Required(ErrorMessage = "This option is required")]
    [Range(1, 2, ErrorMessage = "Invalid value")]

    public int CanAccessCommissionSystem { get; set; }
    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(50, ErrorMessage = "Max length 50 characters")]
    [MinLength(8, ErrorMessage = "Min length 8 characters")]
    public string FullName { get; set; } = null!;
    [Required(ErrorMessage = "Username is required")]
    [MaxLength(16, ErrorMessage = "Max length 16 characters")]
    [MinLength(8, ErrorMessage = "Min length 8 characters")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password is required")]
    [MaxLength(12, ErrorMessage = "Invalid password, should be 8-12 characters")]
    [MinLength(8, ErrorMessage = "Invalid password, should be 8-12 characters")]
    public string Password { get; set; } = null!;

    public ClientPhoneModel UserPhone { set; get; } = new();
    [Required(ErrorMessage = "Hiring date is required")]
    public DateTime? HireDate { get; set; }

    [Required(ErrorMessage = "Company is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid company")]

    public int CompanyId { get; set; }
    [Required(ErrorMessage = "Branch is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid branch")]

    public int BranchId { get; set; }
    [Required(ErrorMessage = "Department is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid department")]

    public int DepartmentId { get; set; }
    [Required(ErrorMessage = "Position is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid position")]

    public int PositionId { get; set; }

    [Required(ErrorMessage = "In resale team is required")]
    [Range(1, 2, ErrorMessage = "Invalid value")]
    public int InResaleTeamId { get; set; }

    [Required(ErrorMessage = "Direct manager is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Invalid direct manager")]

    public int DirectManagerId { get; set; }
}