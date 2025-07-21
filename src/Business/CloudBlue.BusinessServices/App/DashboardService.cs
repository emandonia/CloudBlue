using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.App;
public class DashboardService(IDashboardRepository repo, LoggedInUserInfo loggedInUserInfo, ILookUpsService lookUpsService) : BaseService, IDashboardService
{
    protected override void PopulateInitialData()
    {

    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }
    private const int SalesDepartmentId = 1;
    private const int OperationsDepartmentId = 1;
    public async Task<DashboardContent> GetDashboardContentsAsync()
    {

        var dashboardContent = new DashboardContent();
        var managerId = 0;
        var agentId = 0;
        var companyId = 0;
        var branchId = 0;
        var dashboardNotifications = await lookUpsService.GetDashboardNotificationsAsync();
        DashboardNotificationItem[] currentUserNotifications = [];
        if (loggedInUserInfo.DepartmentId == SalesDepartmentId)
        {
            if (loggedInUserInfo.CanHaveTeam || loggedInUserInfo.IsManager)
            {
                managerId = loggedInUserInfo.UserId;
                currentUserNotifications = dashboardNotifications.Where(x => x.DepartmentId == SalesDepartmentId && x.SalesMangers).ToArray();
            }
            else
            {
                agentId = loggedInUserInfo.UserId;
                currentUserNotifications = dashboardNotifications.Where(x => x.DepartmentId == SalesDepartmentId && x.SalesAgents).ToArray();
            }
        }
        else if (loggedInUserInfo.DepartmentId == OperationsDepartmentId)
        {
            currentUserNotifications = dashboardNotifications.Where(x => x.DepartmentId == OperationsDepartmentId).ToArray();
            companyId = loggedInUserInfo.CompanyId;
            branchId = loggedInUserInfo.BranchId;

        }

        var leadTicketsCounts = await repo.GetLeadTicketsCountAsync(agentId, managerId, branchId, companyId);
        var primeTcrCounts = await repo.GetPrimeTcrCountAsync(agentId, managerId, branchId, companyId);
        if (leadTicketsCounts.Length == 0 && primeTcrCounts.Length == 0)
        {
            return dashboardContent;
        }
        var leadTicketsCountItem = leadTicketsCounts[0];
        dashboardContent.LeadTicketsNotifications = PopulateNotifications(leadTicketsCountItem, currentUserNotifications.Where(z => z.SystemEntityType == EntityTypes.LeadTicket).ToArray());



        var primeCountItem = primeTcrCounts[0];
        dashboardContent.PrimeTcrsNotifications = PopulateNotifications(primeCountItem, currentUserNotifications.Where(z => z.SystemEntityType == EntityTypes.PrimeTcr).ToArray());



        //dashboardContent.LeadTicketsNotifications[0].Count = 0;
        //dashboardContent.LeadTicketsNotifications[1].Count = 8;
        //dashboardContent.LeadTicketsNotifications[2].Count = 15;
        //dashboardContent.LeadTicketsNotifications[3].Count = 1200;


        return dashboardContent;
    }

    private DashboardNotificationItemForList[] PopulateNotifications<T>(
        T countsItem,
        DashboardNotificationItem[] dashboardNotifications)
    {
        var list = new List<DashboardNotificationItemForList>();
        var properties = countsItem?.GetType().GetProperties();
        foreach (var dashboardNotification in dashboardNotifications)
        {
            var count = 0l;
            var notification = new DashboardNotificationItemForList
            {
                Count = 0,
                Label = dashboardNotification.Label,
                LinkUrl = dashboardNotification.LinkUrl,
                Order = dashboardNotification.Order
            };
            list.Add(notification);
            var property = properties.FirstOrDefault(x => x.Name == dashboardNotification.PropertyName);

            if (property == null)
            {
                continue;
            }
            var value = property.GetValue(countsItem);

            if (value == null)
            {
                continue;
            }

            long.TryParse(value.ToString(), out count);
            notification.Count = count;
        }
        return list.ToArray();


    }
}
