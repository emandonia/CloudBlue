namespace CloudBlue.Domain.DataModels.Operations;

public class SalesUserTree : BaseDataModel<int>
{
    public int UserId { get; set; }

    public string UserFullName { get; set; } = null!;

    public int ParentId { get; set; }
}