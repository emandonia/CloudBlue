using CloudBlue.Domain.DomainModels.Users.UserAuth;
using CloudBlue.Domain.DomainModels.Users.UserSessions;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IUsersAuthRepository : IBaseRepository
{
    Task<UserInfoForValidation?> GetUserByUsernameAsync(string username);
    Task<bool> ForgetPassword(LoginItem loginItem);

    Task UpdateFailedPasswordAttemptCount(int userId, int failedPasswordAttemptCount, bool lockedOut);
    Task UpdateSuccessLogin(int userInfoId);
    Task<LoggedInUserInfo?> GetLoggedInUserInfo(int userInfoId);
    Task<bool> CreateUserSession(UserSessionItem userSession);
    Task<UserSessionItem?> GetActiveUserSessionAsync(string apiKey);
    Task SetSessionExpiredAsync(long sessionId, string? apiKey = "");
}