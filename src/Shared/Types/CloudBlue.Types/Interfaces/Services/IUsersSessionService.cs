using CloudBlue.Domain.DomainModels.Users.UserSessions;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IUsersSessionService : IBaseService
{
    Task<string?> CreateUserSession(LoggedInUserInfo loggedInUser, string? loginItemDeviceServiceId,
        string? loginItemLoginProvider);

    Task RemoveSessionAsync(string apiKey);
    Task<LoggedInUserInfo?> GetUserSessionAsync(string apiKey);
}