namespace CloudBlue.Domain.DataModels.CbUsers;

public class UserGroup : BaseDataModel<int>
{
    public string UserGroupName { get; set; } = null!;
}