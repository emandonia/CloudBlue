namespace CloudBlue.Domain.DataModels.CbUsers;

public class PrivilegeScope : BaseDataModel<int>
{
    public string PrivilegeScopeName { get; set; } = null!;

    public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; } = new List<EntityPrivilege>();
}