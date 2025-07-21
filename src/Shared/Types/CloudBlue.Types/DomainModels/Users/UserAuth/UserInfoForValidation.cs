namespace CloudBlue.Domain.DomainModels.Users.UserAuth;

public class UserInfoForValidation : UserBaseInfo
{
    public bool IsParent { get; set; }

    public bool IsVirtual { get; set; }
    public bool CanUserAccessPortal { get; set; }
    public bool CanAccessCommissionSystem { get; set; }
    public string Password { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;
    public int FailedPasswordAttemptCount { get; set; }
}