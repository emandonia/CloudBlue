using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;

namespace CloudBlue.Data.Repositories.App;

public class NotificationsRepository(IAppDataContext appDb) : INotificationsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }
    public async Task AddLeadTicketNotificationLogsAsync(List<LeadTicketNotificationLog> logs)
    {
        await appDb.LeadTicketNotificationLogs.BulkInsertAsync(logs);
    }
}