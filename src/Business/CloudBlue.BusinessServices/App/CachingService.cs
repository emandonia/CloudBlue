using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace CloudBlue.BusinessServices.App;

public class CachingService(IMemoryCache cache) : ICachingService
{
    public string GetItem(string itemKey)
    {
    if (cache.TryGetValue(itemKey, out string? item) && string.IsNullOrEmpty(item) == false)
    {
    return item;
    }

    return string.Empty;
    }

    public bool SaveItem(string itemKey, string item)
    {
    if (string.IsNullOrEmpty(itemKey) || itemKey.Length < 8 || string.IsNullOrEmpty(item) || item.Length < 1)
    {
    return false;
    }

    RemoveItem(itemKey);
    cache.Set(itemKey, item);

    return true;
    }

    public bool SaveLastAgentsRefreshTime()
    {
    var itemKey = LiteralsHelper.LastAgentsRefreshKey;
    RemoveItem(itemKey);
    cache.Set(itemKey, DateTime.UtcNow);

    return true;
    }

    public bool SaveLastPrivilegesRefreshTime()
    {
    var itemKey = LiteralsHelper.LastPrivilegesRefreshKey;
    RemoveItem(itemKey);
    cache.Set(itemKey, DateTime.UtcNow);

    return true;
    }

    public DateTime? GetLastPrivilegesRefreshTime()
    {
    var itemKey = LiteralsHelper.LastPrivilegesRefreshKey;

    if (cache.TryGetValue(itemKey, out DateTime? item) && item != null)
    {
    return item;
    }

    return null;
    }

    public DateTime? GetLastAgentsRefreshTime()
    {
    var itemKey = LiteralsHelper.LastAgentsRefreshKey;

    if (cache.TryGetValue(itemKey, out DateTime? item) && item != null)
    {
    return item;
    }

    return null;
    }

    public bool RemoveItem(string itemKey)
    {
    if (cache.TryGetValue(itemKey, out string? _))
    {
    cache.Remove(itemKey);
    }

    return true;
    }
}