using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.UsersAccounts;

public class UsersService(
    IUsersRepository repo,
    ICachingService cachingService, INotificationsService notificationsService,
    LoggedInUserInfo loggedInUserInfo,
    IUsersDataService usersDataService) : BaseService, IUsersService
{
    private readonly int _salesDepartmentId = 1;

    public async Task<ListResult<UserItemForList>> GetUsersAsync(UsersFiltersModel filters)
    {
        return await repo.GetUsersAsync(filters);
    }

    public async Task<bool> UpdateAccountStatusAsync(int userId, bool status)
    {
        return await repo.UpdateAccountStatusAsync(userId, status);
    }

    public async Task<bool> UnblockAccountAsync(int userId)
    {
        return await repo.UnblockAccountAsync(userId);
    }

    public async Task<bool> UpdateManagersArraysAsync()
    {
        var users = await repo.GetSalesUsersEntitiesAsync();

        foreach (var user in users)
        {
            //if (user.DirectManagerId == 0)
            //{
            //    user.ManagersIdsArray = [user.Id];
            //    continue;
            //}
            var manager = await usersDataService.GetUserTreeAsync(user.Id);

            if (manager == null)
            {
                continue;

            }

            user.ManagersIdsArray = manager.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray();

        }
        await repo.UpdateUsersAsync(users);

        return true;

    }

    public async Task<bool> UpdatePositionAsync(int userId, int positionId, DateTime promotionDate)
    {
        var result = await repo.UpdateUserPositionAsync(userId, positionId);

        if (result == false)
        {
            return false;
        }

        if (await repo.IsUserInDepartmentAsync(userId, _salesDepartmentId) == false)
        {
            return true;
        }

        await repo.EndUserPromotionRecordsAsync(userId, UtilityFunctions.GetUtcDateTime(promotionDate.AddDays(-1)));

        return await repo.CreateUserPromotionRecordAsync(userId, positionId,
            UtilityFunctions.GetUtcDateTime(promotionDate));
    }

    public async Task<bool> UpdateDirectManagerAsync(int userId, int managerId)
    {
        if (await repo.IsUserInDepartmentAsync(userId, _salesDepartmentId) == false)
        {
            return false;
        }
        var result = await repo.UpdateUserSalesTreeAsync(userId, string.Empty, managerId);
        usersDataService.ForceRefresh = true;
        var userTree = await usersDataService.GetUserTreeAsync(userId);

        if (userTree == null || userTree.SalesUsers.SalesUsers.Any(z => z.UserId != userId) == false)
        {
            return false;
        }

        var manager = userTree.SalesUsers.SalesUsers.OrderBy(z => z.Level)
            .FirstOrDefault(z => z.UserId != userId);


        if (manager == null)
        {
            return false;
        }

        var managersArray = userTree.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray();

        if (userTree.SalesUsers.SalesUsers.Count(z => z.UserId != userId) > 0)
        {
            var topManager = userTree.SalesUsers.SalesUsers.OrderBy(z => z.Level).Last(z => z.UserId != userId);

            return await repo.UpdateDirectManagerAsync(userId, manager.UserId, manager.AgentName, topManager.UserId,
                topManager.AgentName, managersArray);
        }

        return await repo.UpdateDirectManagerAsync(userId, manager.UserId, manager.AgentName, manager.UserId,
            manager.AgentName, managersArray);


    }

    public async Task<bool> ChangePasswordAsync(int userId, string password)
    {
        var salt = UtilityFunctions.GenerateSalt();
        var hashedPassword = UtilityFunctions.HashPassword(password.ToLower(), salt);

        return await repo.ChangePasswordAsync(userId, hashedPassword, salt);
    }

    public async Task<bool> SetResignedAsync(int userId, DateTime resignDate)
    {
        return await repo.SetResignedAsync(userId, resignDate);
    }

    public async Task<bool> CreateUserAsync(CreateUserModel model)
    {
        model.Email = model.Email.ToLower();
        model.Username = model.Username.ToLower();

        model.UserPhone.DeviceInfo = UtilityFunctions.GetCompositePhoneForSearch(model.UserPhone.CountryCode,
            model.UserPhone.AreaCode, model.UserPhone.Phone);

        var exists = await repo.CheckUserExistAsync(model.Username, model.Email, model.UserPhone.DeviceInfo);

        if (exists.Length > 0)
        {
            PopulateErrors(model, exists);

            return false;
        }

        var fullModel = new CreateUserFullModel
        {
            UserPhone = model.UserPhone,
            DepartmentId = model.DepartmentId,
            CompanyId = model.CompanyId,
            BranchId = model.BranchId,
            PositionId = model.PositionId,
            Username = model.Username,
            Email = model.Email,
            FullName = model.FullName,
            DirectManagerId = model.DirectManagerId,
            FullNameLowered = model.FullName.ToLower(),
            HireDate = UtilityFunctions.GetUtcDateTime(model.HireDate.Value),
            HireDateNumeric = int.Parse(model.HireDate.Value.ToString("yyyyMMdd")),
            InResaleTeam = model.InResaleTeamId == 1,
            IsParent = false,
            IsVirtual = false,
            ParentId = 0,
            CanAccessCommissionSystem = model.CanAccessCommissionSystem,
            CanUserAccessPortal = model.CanUserAccessPortal,
        };

        fullModel.LastPromotionDate = fullModel.HireDate;
        fullModel.LastPromotionDateNumeric = int.Parse(fullModel.HireDate.Value.ToString("yyyyMMdd"));
        fullModel.PasswordSalt = UtilityFunctions.GenerateSalt();
        fullModel.Password = UtilityFunctions.HashPassword(model.Password.ToLower(), fullModel.PasswordSalt);

        if (fullModel.DepartmentId == _salesDepartmentId)
        {
            var tree = await usersDataService.GetUserTreeAsync(fullModel.DirectManagerId);

            if (tree == null || tree.SalesUsers.SalesUsers.Count == 0)
            {
                return false;
            }


            fullModel.DirectManagerName = tree.SalesUsers.SalesUsers.First()
                .AgentName;

            var managersArray = tree.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray();
            fullModel.ManagersArray = managersArray;

            var topManager = tree.SalesUsers.SalesUsers.Last();
            fullModel.TopMostManagerName = topManager.AgentName;
            fullModel.TopMostManagerId = topManager.UserId;
        }

        var result = await repo.CreateUserAsync(fullModel);

        if (result == false || model.DepartmentId != _salesDepartmentId)
        {
            return result;
        }

        var userId = Convert.ToInt32(repo.LastCreatedItemId);
        await repo.UpdateUserSalesTreeAsync(userId, model.FullName, model.DirectManagerId);
        await repo.CreateUserPromotionRecordAsync(userId, fullModel.PositionId, fullModel.HireDate.Value);

        return result;
    }

    public async Task<bool> UpdateEmailAsync(int userId, string email)
    {
        email = email.ToLower();
        var exists = await repo.CheckUserExistAsync(string.Empty, email, string.Empty);

        if (exists.Length > 0 && exists.Any(z => z.UserId != userId))
        {
            LastErrors.Add(Errors.EmailAlreadyExists);

            return false;
        }

        return await repo.UpdateEmailAsync(userId, email);
    }

    public async Task<bool> UpdateFullNameAsync(int userId, string fullName)
    {
        return await repo.UpdateFullNameAsync(userId, fullName);
    }

    public async Task<bool> UpdateMobilAsync(int userId, ClientPhoneModel userMobile)
    {
        userMobile.DeviceInfo = UtilityFunctions.GetCompositePhoneForSearch(userMobile.CountryCode,
            userMobile.AreaCode, userMobile.Phone);

        var exists = await repo.CheckUserExistAsync(string.Empty, string.Empty, userMobile.DeviceInfo);

        if (exists.Length > 0 && exists.Any(z => z.UserId != userId))
        {
            LastErrors.Add(Errors.MobileAlreadyExists);

            return false;
        }

        return await repo.UpdateUserMobileAsync(userId, userMobile);
    }

    public async Task<bool> UpdateUsernameAsync(int userId, string userName)
    {
        var exists = await repo.CheckUserExistAsync(userName, string.Empty, string.Empty);

        if (exists.Length > 0 && exists.Any(z => z.UserId != userId))
        {
            LastErrors.Add(Errors.UserNameAlreadyExists);

            return false;
        }

        return await repo.UpdateUsernameAsync(userId, userName);
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordModel model)
    {

        var user = await repo.GetUserEntityAsync(loggedInUserInfo.UserId);

        if (user == null)
        {
            LastErrors.Add(Errors.UserDoesNotExist);
            return false;
        }


        var hashedPassword = UtilityFunctions.HashPassword(model.OldPassword.ToLower(), user.PasswordSalt);

        if (string.IsNullOrEmpty(hashedPassword))
        {
            LastErrors.Add(Errors.InvalidOldPassword);

            return false;
        }

        var salt = UtilityFunctions.GenerateSalt();
        hashedPassword = UtilityFunctions.HashPassword(model.Password.ToLower(), salt);

        return await repo.ChangePasswordAsync(loggedInUserInfo.UserId, hashedPassword, salt);
    }


    public async Task<bool> ResetUserPasswordAsync(int userId)
    {

        var user = await repo.GetUserEntityAsync(userId);

        if (user == null)
        {
            LastErrors.Add(Errors.UserDoesNotExist);

            return false;
        }

        var password = Guid.NewGuid()
            .ToString()
            .Replace("-", "").Substring(0, 12).ToLower();


        var salt = UtilityFunctions.GenerateSalt();
        var hashedPassword = UtilityFunctions.HashPassword(password, salt);

        var result = await repo.ChangePasswordAsync(loggedInUserInfo.UserId, hashedPassword, salt);

        if (result)
        {

            var mobile = string.Empty;
            var rec = user.UserPhones.FirstOrDefault(z => z.DeviceTypeId != 1);

            if (rec != null)
            {
                mobile = rec.DeviceInfo;
            }


            await notificationsService.SendResetPasswordAsync(password, user.Email, user.FullName, userId, mobile);
        }

        return true;



    }


    protected override void PopulateInitialData()
    {
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privileges)
    {
        return null;
    }

    private void PopulateErrors(CreateUserModel model, UserExistingResult[] exists)
    {
        if (exists.Any(z => z.Email == model.Email))
        {
            LastErrors.Add(Errors.EmailAlreadyExists);
        }

        if (exists.Any(z => z.Username == model.Username))
        {
            LastErrors.Add(Errors.UserNameAlreadyExists);
        }

        if (exists.Any(z => z.Mobile == model.UserPhone.DeviceInfo))
        {
            LastErrors.Add(Errors.UserNameAlreadyExists);
        }
    }
}