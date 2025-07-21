using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DataModels.CbUsers;

public class VwEntityPrivilege : BaseDataModel<long>
{
    public SystemPrivileges Privilege { get; set; }

    public int EntityId { get; set; }

    public PrivilegeScopes PrivilegeScope { get; set; }

    public PrivilegeEntityTypes PrivilegeEntityType { get; set; }

    public string? PrivilegeCategoryName { get; set; }

    public string? PrivilegeEntityTypeName { get; set; }

    public string? PrivilegeName { get; set; }

    public PrivilegeCategories PrivilegeCategory { get; set; }

    public string? PrivilegeMetaData { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }
    public bool AccessOnly { get; set; }

    public string? Path { get; set; }

    public string? PrivilegeScopeName { get; set; }
    public string? EntityName { get; set; }
}