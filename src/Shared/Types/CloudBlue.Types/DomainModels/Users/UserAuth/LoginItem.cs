using System.ComponentModel.DataAnnotations;

namespace CloudBlue.Domain.DomainModels.Users.UserAuth;

public class LoginItem
{
	[Required]
	[MinLength(6)]
	[MaxLength(50)]
	public string Username { get; set; } = null!;
	[Required]
	[MinLength(8)]
	[MaxLength(20)]
	public string Password { get; set; } = null!;
	[Required]

	public string? LoginProvider { get; set; } = null!;
	[Required]

	public string? DeviceServiceId { get; set; } = null!;
}