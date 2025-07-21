namespace CloudBlue.Domain.DomainModels.DashboardStuff;

public class DashboardNotificationItemForList
{

    public long Count { set; get; }
    public String Label { set; get; } = null!;
    public int Order { set; get; }
    public string? LinkUrl { set; get; }
    public object StatusBadgeStyle { get; set; } = null!;

}