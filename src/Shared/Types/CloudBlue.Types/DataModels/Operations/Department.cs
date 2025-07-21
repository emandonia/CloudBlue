namespace CloudBlue.Domain.DataModels.Operations;

public class Department
{
    public int Id { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<UserPosition> UserPositions { get; set; } = new List<UserPosition>();
}