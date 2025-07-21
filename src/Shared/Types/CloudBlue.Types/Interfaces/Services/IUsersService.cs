using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IUsersService : IBaseService
{
    Task<bool> ResetUserPasswordAsync(int userId);


    Task<ListResult<UserItemForList>> GetUsersAsync(UsersFiltersModel filters);
    Task<bool> UpdateAccountStatusAsync(int userId, bool status);
    Task<bool> UnblockAccountAsync(int userId);
    Task<bool> UpdateManagersArraysAsync();
    Task<bool> UpdatePositionAsync(int userId, int positionId, DateTime promotionDate);
    Task<bool> UpdateDirectManagerAsync(int userId, int managerId);
    Task<bool> ChangePasswordAsync(int userId, string password);
    Task<bool> SetResignedAsync(int userId, DateTime resignDate);
    Task<bool> CreateUserAsync(CreateUserModel model);
    Task<bool> UpdateEmailAsync(int userId, string email);
    Task<bool> UpdateFullNameAsync(int userId, string fullName);
    Task<bool> UpdateMobilAsync(int userId, ClientPhoneModel userMobile);
    Task<bool> UpdateUsernameAsync(int userId, string userName);
    Task<bool> ChangePasswordAsync(ChangePasswordModel model);
}