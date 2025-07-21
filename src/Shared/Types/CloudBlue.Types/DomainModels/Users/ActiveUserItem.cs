namespace CloudBlue.Domain.DomainModels.Users;
public class ActiveUserItem
{
    public string? FullNameLowered { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public int UserId { get; set; }
}
