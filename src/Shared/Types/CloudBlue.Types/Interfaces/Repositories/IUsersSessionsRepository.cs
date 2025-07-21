using CloudBlue.Domain.DomainModels.Users.UserSessions;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IUsersSessionsRepository : IBaseRepository
{
    Task<bool> CreateUserSession(UserSessionItem userSession);
    Task<UserSessionItem?> GetActiveUserSessionAsync(string apiKey);
    Task SetSessionExpiredAsync(long sessionId, string? apiKey = "");
}