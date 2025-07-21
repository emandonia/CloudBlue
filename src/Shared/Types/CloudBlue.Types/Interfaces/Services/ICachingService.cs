namespace CloudBlue.Domain.Interfaces.Services;

public interface ICachingService
{
    string GetItem(string itemKey);
    bool SaveItem(string itemKey, string item);
    bool SaveLastAgentsRefreshTime();
    bool SaveLastPrivilegesRefreshTime();
    DateTime? GetLastPrivilegesRefreshTime();
    DateTime? GetLastAgentsRefreshTime();
    bool RemoveItem(string itemKey);
}