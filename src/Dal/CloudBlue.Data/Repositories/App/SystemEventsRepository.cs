using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;

namespace CloudBlue.Data.Repositories.App;

public class SystemEventsRepository(IAppDataContext appDb) : ISystemEventsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public SystemEventTemplate[] GetSystemEventTemplatesAsync()
    {
        return appDb.SystemEventTemplates.ToArray();
    }

    public async Task CreateEventsAsync(List<SystemEvent> systemEvents)
    {
        foreach (var systemEvent in systemEvents)
        {
            await appDb.SystemEvents.AddAsync(systemEvent);
            await appDb.SaveChangesAsync();
        }
    }

    public async Task CreateBulkEventsAsync(IEnumerable<SystemEvent> systemEvents)
    {
        await appDb.SystemEvents.AddRangeAsync(systemEvents);
        await appDb.SaveBulkChangesAsync();
    }

    public async Task UpdateEntityEventsAsync(EntityTypes entityType, long entityId, int userId = 0,
        long leadTicketId = 0)
    {
        await appDb.UpdateEntityLastEventsAsync(entityType, entityId, userId, leadTicketId);
    }
}