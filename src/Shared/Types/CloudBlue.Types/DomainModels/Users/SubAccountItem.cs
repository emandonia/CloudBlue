namespace CloudBlue.Domain.DomainModels.Users;

public class SubAccountItem
{
	public long UserId { get; set; }
	public string? UserFullName { get; set; } = null!;
	public string? UserPosition { get; set; } = null!;
}