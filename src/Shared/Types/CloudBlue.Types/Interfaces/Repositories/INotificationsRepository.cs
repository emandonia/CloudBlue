using CloudBlue.Domain.DataModels;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface INotificationsRepository : IBaseRepository
{
    Task AddLeadTicketNotificationLogsAsync(List<LeadTicketNotificationLog> logs);
}