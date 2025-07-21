namespace CloudBlue.Domain.DataModels.CbUsers;

public class PrivilegeCategory : BaseDataModel<int>
{
    public string PrivilegeCategoryName { get; set; } = null!;

    public virtual ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();
}