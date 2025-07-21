namespace CloudBlue.Domain.DataModels;

public class DashboardNotification : BaseDataModel<int>
{

    public int SystemEntityId { get; set; }

    public string Label { get; set; } = null!;

    public int Order { get; set; }
    public string? LinkUrl { get; set; }

    public string PropertyName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public bool SalesMangers { get; set; }

    public bool SalesAgents { get; set; }

    public int PositionId { get; set; }
}
