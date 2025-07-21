namespace CloudBlue.Domain.DataModels.CbUsers;

public class PrivilegeEntityType : BaseDataModel<int>
{
    public string PrivilegeEntityTypeName { get; set; } = null!;

    public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; } = new List<EntityPrivilege>();
}