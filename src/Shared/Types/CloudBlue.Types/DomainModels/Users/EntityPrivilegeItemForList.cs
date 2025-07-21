using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.Users;

public class EntityPrivilegeItemForList
{


    public string? PrivilegeCategoryName { get; set; }

    public string? PrivilegeEntityTypeName { get; set; }

    public string? PrivilegeName { get; set; }


    public string? PrivilegeMetaData { get; set; }
    public string? EntityName { get; set; }

    public string? PrivilegeScopeName { get; set; }
    public PrivilegeScopes PrivilegeScope { get; set; }
    public long Id { get; set; }
}