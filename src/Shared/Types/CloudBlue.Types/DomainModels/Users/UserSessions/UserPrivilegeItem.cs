using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.Users.UserSessions;

public class UserPrivilegeItem : PrivilegeItem
{
    public PrivilegeScopes PrivilegeScope { get; set; }
    public PrivilegeEntityTypes PrivilegeEntityType { get; set; }
    public long Id { get; set; }
    public bool AccessOnly { get; set; }
}