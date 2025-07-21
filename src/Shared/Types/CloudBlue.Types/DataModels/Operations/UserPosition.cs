namespace CloudBlue.Domain.DataModels.Operations;

public class UserPosition
{
    public int Id { get; set; }

    public string UserPositionName { get; set; } = null!;

    public int PositionOrder { get; set; }

    public bool ApplyTargets { get; set; }

    public bool IsManager { get; set; }

    public bool CanHaveTeam { get; set; }

    public int DepartmentId { get; set; }

    public int UserGroupId { get; set; }

    public bool HasLineOfBusiness { get; set; }
}