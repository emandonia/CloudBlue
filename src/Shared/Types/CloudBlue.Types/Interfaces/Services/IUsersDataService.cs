using CloudBlue.Domain.DomainModels.Users;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IUsersDataService : IBaseService
{
    Task<UserTreeItem?> GetUserTreeAsync(int userId);
    bool ForceRefresh { set; get; }
}