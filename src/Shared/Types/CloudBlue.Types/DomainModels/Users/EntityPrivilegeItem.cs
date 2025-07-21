using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels.Users;

public class EntityPrivilegeItem
{
    public SystemPrivileges Privilege { get; set; }

    public int EntityId { get; set; }
    public bool AccessOnly { get; set; }

    public PrivilegeScopes PrivilegeScope { get; set; }
    public int PrivilegeScopeId { get; set; }

    public PrivilegeEntityTypes PrivilegeEntityType { get; set; }

    public PrivilegeCategories PrivilegeCategory { get; set; }

    public string? ControllerName { get; set; } = null!;

    public string? ActionName { get; set; } = null!;

    public string? Path { get; set; } = null!;
    public long Id { get; set; }
    public int PrivilegeEntityTypeId { get; set; }
    public string? PrivilegeMetaData { get; set; }
}