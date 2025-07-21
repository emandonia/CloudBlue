using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.Users.UserSessions;

public class PrivilegeItem
{
    public SystemPrivileges Privilege { get; set; }
    public PrivilegeCategories PrivilegeCategory { get; set; }

    public string? PrivilegeMetaData { get; set; }
    public string? ActionName { set; get; }
    public string? ControllerName { set; get; }
    public string? Path { set; get; }
}