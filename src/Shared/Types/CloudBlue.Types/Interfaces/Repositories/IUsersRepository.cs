using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IUsersRepository : IBaseRepository
{
    Task<ListResult<UserItemForList>> GetUsersAsync(UsersFiltersModel filters);
    Task<bool> UpdateAccountStatusAsync(int userId, bool status);
    Task<bool> UnblockAccountAsync(int userId);
    Task<bool> UpdateUserPositionAsync(int userId, int positionId);

    Task<bool> UpdateDirectManagerAsync(int userId, int directManagerId, string? directManagerName, int topManagerId,
        string? topManagerName, int[] managersArray);

    Task<bool> UpdateUserSalesTreeAsync(int userId, string userFullName, int directManagerId);
    Task<bool> ChangePasswordAsync(int userId, string password, string passwordSalt);

    Task<User?> GetUserEntityAsync(int userId);
    Task<SalesPromotion[]> GetUserPromotionEntitiesAsync(int userId, bool onlyActive);
    Task<bool> CreateUserPromotionRecordAsync(int userId, int positionId, DateTime promotionStartDate);
    Task<bool> EndUserPromotionRecordsAsync(int userId, DateTime promotionEndDate);
    Task<UserTreeItem[]> GetSalesUserTreeItemsAsync();
    Task<bool> IsUserInDepartmentAsync(int userId, int departmentId);
    Task<bool> SetResignedAsync(int userId, DateTime resignDate);
    Task<UserExistingResult[]> CheckUserExistAsync(string username, string email, string mobile);
    Task<bool> CreateUserAsync(CreateUserFullModel fullModel);
    Task<UserPosition[]> GetSalesUserPositionsEntitiesAsync();
    Task<bool> UpdateFullNameAsync(int userId, string fullName);
    Task<bool> UpdateEmailAsync(int userId, string email);
    Task<bool> UpdateUserMobileAsync(int userId, ClientPhoneModel userMobile);
    Task<bool> UpdateUsernameAsync(int userId, string userName);
    Task<User[]> GetSalesUsersEntitiesAsync();
    Task UpdateUsersAsync(User[] users);
}