using Blazored.LocalStorage;

namespace CloudBlue.Web.Helpers;

public interface ILocalStorageHelper
{
    Task<bool> StoreItem(string key, string value);

    Task<string> RetrieveItem(string key);

    Task<bool> RemoveItem(string key);

    Task<bool> ClearStorage();
}

public class LocalStorageHelper(ILocalStorageService localStorage) : ILocalStorageHelper
{
    public async Task<bool> StoreItem(string key, string value)
    {
        await localStorage.SetItemAsStringAsync(key, value);

        return true;
    }

    public async Task<string> RetrieveItem(string key)
    {
        return await localStorage.GetItemAsync<string>(key) ?? string.Empty;
    }

    public async Task<bool> RemoveItem(string key)
    {
        await localStorage.RemoveItemAsync(key);

        return true;
    }

    public async Task<bool> ClearStorage()
    {
        await localStorage.ClearAsync();

        return true;
    }
}