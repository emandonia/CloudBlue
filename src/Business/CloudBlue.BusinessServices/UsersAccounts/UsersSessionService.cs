using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.UsersAccounts;

public class UsersSessionService(
    IApiKeyService apiKeyService,
    IUsersSessionsRepository repo,
    ICachingService cachingService) : BaseService, IUsersSessionService
{
    public async Task<LoggedInUserInfo?> GetUserSessionAsync(string apiKey)
    {
        var item = cachingService.GetItem(apiKey);

        if (string.IsNullOrEmpty(item) == false)
        {
            var userSession = UtilityFunctions.DeserializeJsonString<UserSessionItem>(item);

            if (userSession.ExpireDate.Subtract(DateTime.UtcNow)
                   .Minutes < 5)
            {
                await repo.SetSessionExpiredAsync(userSession.Id);

                return null;
            }

            var loggedInUserInfo = UtilityFunctions.DeserializeJsonString<LoggedInUserInfo>(userSession.SerializedObject);

            return loggedInUserInfo;
        }

        var session = await repo.GetActiveUserSessionAsync(apiKey);

        if (session == null)
        {
            return null;
        }

        if (session.ExpireDate.Subtract(DateTime.UtcNow)
               .Minutes < 5)
        {
            await repo.SetSessionExpiredAsync(session.Id);

            return null;
        }

        cachingService.SaveItem(apiKey, UtilityFunctions.SerializeToJsonString(session));
        var obj = UtilityFunctions.DeserializeJsonString<LoggedInUserInfo>(session.SerializedObject);

        return obj;
    }

    public async Task<string?> CreateUserSession(LoggedInUserInfo loggedInUser, string? deviceServiceId,
        string? loginProvider)
    {
        var apiKey = apiKeyService.GenerateApiKey();

        var userSession = new UserSessionItem
        {
            UserId = loggedInUser.UserId,
            DeviceServiceId = deviceServiceId,
            ExpireDate = DateTime.UtcNow.AddHours(24),
            BranchId = loggedInUser.BranchId,
            CompanyId = loggedInUser.CompanyId,
            LoginProvider = loginProvider,
            SerializedObject = UtilityFunctions.SerializeToJsonString(loggedInUser),
            ApiKey = apiKey
        };

        if (await repo.CreateUserSession(userSession))
        {
            cachingService.SaveItem(apiKey, UtilityFunctions.SerializeToJsonString(userSession));

            return apiKey;
        }

        return null;
    }

    public async Task RemoveSessionAsync(string apiKey)
    {
        cachingService.RemoveItem(apiKey);
        await repo.SetSessionExpiredAsync(0, apiKey);
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }

    protected override void PopulateInitialData()
    {
    }
}