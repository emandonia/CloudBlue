namespace CloudBlue.Domain.DataModels.CbUsers;

public class Privilege : BaseDataModel<int>
{
    public string PrivilegeName { get; set; } = null!;

    public int PrivilegeCategoryId { get; set; }
    public bool AccessOnly { get; set; }

    public string? PrivilegeMetaData { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? Path { get; set; }

    public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; } = new List<EntityPrivilege>();

    public virtual PrivilegeCategory PrivilegeCategory { get; set; } = null!;
}