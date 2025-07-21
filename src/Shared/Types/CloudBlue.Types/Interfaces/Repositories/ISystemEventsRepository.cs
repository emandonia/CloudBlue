using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface ISystemEventsRepository : IBaseRepository
{
    SystemEventTemplate[] GetSystemEventTemplatesAsync();
    Task CreateEventsAsync(List<SystemEvent> systemEvents);
    Task CreateBulkEventsAsync(IEnumerable<SystemEvent> systemEvents);
    Task UpdateEntityEventsAsync(EntityTypes entityType, long entityId, int userId = 0, long leadTicketId = 0);
}