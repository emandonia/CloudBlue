namespace CloudBlue.Domain.DomainModels.DashboardStuff;
public class DashboardContent
{
    public DashboardNotificationItemForList[] LeadTicketsNotifications { get; set; } = [];
    public DashboardNotificationItemForList[] PrimeTcrsNotifications { get; set; } = [];

}