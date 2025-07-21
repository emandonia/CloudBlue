namespace CloudBlue.Domain.DataModels.CbUsers;

public class EntityPrivilege : BaseDataModel<int>
{
    public int PrivilegeId { get; set; }

    public int EntityId { get; set; }
    public string? EntityName { get; set; }

    public int PrivilegeScopeId { get; set; }

    public int PrivilegeEntityTypeId { get; set; }

    public virtual Privilege Privilege { get; set; } = null!;

    public virtual PrivilegeEntityType PrivilegeEntityType { get; set; } = null!;

    public virtual PrivilegeScope PrivilegeScope { get; set; } = null!;
}