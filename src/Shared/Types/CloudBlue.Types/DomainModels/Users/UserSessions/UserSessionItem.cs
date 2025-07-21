namespace CloudBlue.Domain.DomainModels.Users.UserSessions;

public class UserSessionItem
{
    public string? DeviceServiceId { get; set; } = null!;
    public DateTime ExpireDate { set; get; }

    public string ApiKey { get; set; } = null!;
    public string SerializedObject { get; set; } = null!;
    public int UserId { get; set; }
    public string? LoginProvider { get; set; } = null!;
    public int BranchId { get; set; }
    public int CompanyId { get; set; }
    public long Id { get; set; }
}