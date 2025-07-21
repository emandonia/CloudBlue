using CloudBlue.Domain.DomainModels.Users.UserAuth;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IUsersAuthService : IBaseService
{
    Task<string?> SignIn(LoginItem loginItem, bool b = false);
    Task<bool> ForgetPassword(LoginItem loginItem);
    Task<bool> PopulateUserSessionAsync(string apiKey);
    Task RemoveSessionAsync(string apiKey);
}