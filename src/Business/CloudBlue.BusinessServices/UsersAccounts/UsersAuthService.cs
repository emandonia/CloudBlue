using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserAuth;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.UsersAccounts;

public class UsersAuthService(
    IUsersSessionService sessionService,
    IUsersAuthRepository repo,
    IPrivilegesService privilegesService,
    LoggedInUserInfo loggedInUserInfo,
    ILookUpsService lookupService,
    ICachingService cachingService) : BaseService, IUsersAuthService
{
    private EntityPrivilegeItem[] _entityPrivileges = [];

    public async Task<string?> SignIn(LoginItem loginItem, bool b = false)
    {
        LastErrors.Clear();
        var userInfo = await repo.GetUserByUsernameAsync(loginItem.Username.ToLower());

        if (userInfo == null)
        {
            LastErrors.Add(Errors.UserDoesNotExist);

            return null;
        }

        if (userInfo.IsApproved == false || userInfo.IsLockedOut || userInfo.CanUserAccessPortal == false)
        {
            LastErrors.Add(Errors.AccountIsNotActiveContactAdmin);

            return string.Empty;
        }

        var hashedPassword = UtilityFunctions.HashPassword(loginItem.Password.ToLower(), userInfo.PasswordSalt);

        if (string.IsNullOrEmpty(hashedPassword))
        {
            LastErrors.Add(Errors.AccountIsNotActiveContactAdmin);

            return string.Empty;
        }

        var allowedFailedPassword = GetAllowedFailedPassword();
        var lockedOut = userInfo.FailedPasswordAttemptCount >= allowedFailedPassword;

        if (b == false && (hashedPassword != userInfo.Password || lockedOut))
        {
            LastErrors.Add(Errors.InvalidCredentials);
            userInfo.FailedPasswordAttemptCount++;
            lockedOut = userInfo.FailedPasswordAttemptCount >= allowedFailedPassword;
            await repo.UpdateFailedPasswordAttemptCount(userInfo.Id, userInfo.FailedPasswordAttemptCount, lockedOut);

            return null;
        }

        await repo.UpdateSuccessLogin(userInfo.Id);
        var loggedInUser = await repo.GetLoggedInUserInfo(userInfo.Id);

        if (loggedInUser == null)
        {
            LastErrors.Add(Errors.UserDoesNotExist);

            return null;
        }

        PopulateLoggedInUserInfo(loggedInUser);
        await PopulateLoggedInUserPrivileges();
        await PopulateUserDownTree();

        return await sessionService.CreateUserSession(loggedInUserInfo, loginItem.DeviceServiceId,
            loginItem.LoginProvider);
    }

    public Task<bool> ForgetPassword(LoginItem loginItem)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> PopulateUserSessionAsync(string apiKey)
    {
        var userInfo = await sessionService.GetUserSessionAsync(apiKey);

        if (userInfo == null)
        {
            return false;
        }

        PopulateLoggedInUserInfo(userInfo);
        await PopulateLoggedInUserPrivileges();
        await PopulateUserDownTree();

        return true;
    }

    public async Task RemoveSessionAsync(string apiKey)
    {
        await sessionService.RemoveSessionAsync(apiKey);
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    private async Task PopulateLoggedInUserPrivileges()
    {
        await PopulatePrivilegesList();

        var userPrivileges = _entityPrivileges.Where(z =>
                (z.EntityId == loggedInUserInfo.UserId && z.PrivilegeEntityType == PrivilegeEntityTypes.User) ||
                (z.EntityId == loggedInUserInfo.PositionId && z.PrivilegeEntityType == PrivilegeEntityTypes.Position) ||
                (z.EntityId == loggedInUserInfo.DepartmentId &&
                 z.PrivilegeEntityType == PrivilegeEntityTypes.Department))
            .ToArray();

        var distinctPrivileges = userPrivileges.Select(z => z.Privilege)
            .Distinct()
            .ToArray();

        var lst = new List<UserPrivilegeItem>();

        foreach (var itemPrivilege in distinctPrivileges)
        {
            var item = userPrivileges.Where(z => z.Privilege == itemPrivilege)
                .OrderByDescending(z => z.PrivilegeEntityTypeId)
                .FirstOrDefault();

            if (item == null || item.PrivilegeScope == PrivilegeScopes.Denied)
            {
                continue;
            }

            lst.Add(new UserPrivilegeItem
            {
                Privilege = item.Privilege,
                PrivilegeScope = item.PrivilegeScope,
                PrivilegeMetaData = item.PrivilegeMetaData,
                ControllerName = item.ControllerName,
                ActionName = item.ActionName,
                Path = item.Path,
                PrivilegeEntityType = item.PrivilegeEntityType,
                PrivilegeCategory = item.PrivilegeCategory,
                Id = item.Id,
                AccessOnly = item.AccessOnly
            });
        }

        loggedInUserInfo.Privileges = lst.ToArray();
    }

    private void PopulateLoggedInUserInfo(LoggedInUserInfo loggedInUser)
    {
        loggedInUserInfo.SubAccounts = loggedInUser.SubAccounts.Select(z => new SubAccountItem
        {
            UserFullName = z.UserFullName,
            UserId = z.UserId,
            UserPosition = z.UserPosition
        })
            .ToArray();

        loggedInUserInfo.BranchId = loggedInUser.BranchId;
        loggedInUserInfo.AgentsIds = loggedInUser.AgentsIds;
        loggedInUserInfo.BranchName = loggedInUser.BranchName;
        loggedInUserInfo.CanHaveTeam = loggedInUser.CanHaveTeam;
        loggedInUserInfo.CompanyId = loggedInUser.CompanyId;
        loggedInUserInfo.CurrentAccessPrivilege = loggedInUser.CurrentAccessPrivilege;
        loggedInUserInfo.DepartmentId = loggedInUser.DepartmentId;
        loggedInUserInfo.DirectManagerId = loggedInUser.DirectManagerId;
        loggedInUserInfo.Email = loggedInUser.Email;
        loggedInUserInfo.FullName = loggedInUser.FullName;
        loggedInUserInfo.HasLineOfBusiness = loggedInUser.HasLineOfBusiness;
        loggedInUserInfo.InResaleTeam = loggedInUser.InResaleTeam;
        loggedInUserInfo.IsManager = loggedInUser.IsManager;
        loggedInUserInfo.IsParent = loggedInUser.IsParent;
        loggedInUserInfo.ParentId = loggedInUser.ParentId;
        loggedInUserInfo.PositionId = loggedInUser.PositionId;
        loggedInUserInfo.Privileges = loggedInUser.Privileges;
        loggedInUserInfo.TopMostManagerId = loggedInUser.TopMostManagerId;
        loggedInUserInfo.UserGroupId = loggedInUser.UserGroupId;
        loggedInUserInfo.UserId = loggedInUser.UserId;
        loggedInUserInfo.UserName = loggedInUser.UserName;
        loggedInUserInfo.CompanyName = loggedInUser.CompanyName;
        loggedInUserInfo.UserPositionName = loggedInUser.UserPositionName;
        loggedInUserInfo.LastUpdated = DateTime.UtcNow;
    }

    private async Task PopulateUserDownTree()
    {
        if (loggedInUserInfo.DepartmentId != (int)Departments.Sales)
        {
            return;
        }

        if (loggedInUserInfo.CanHaveTeam == false && loggedInUserInfo.IsManager == false)
        {
            loggedInUserInfo.AgentsIds = [loggedInUserInfo.UserId];

            return;
        }

        var allActiveAgents = await lookupService.GetAgentsAsync(false);

        if (allActiveAgents.Length == 0)
        {
            return;
        }

        loggedInUserInfo.AgentsIds = allActiveAgents.Select(z => z.AgentId)
            .Distinct()
            .ToArray();

        loggedInUserInfo.MangersIds = allActiveAgents.Where(z => z.SalesPersonClass != SalesPersonClasses.Agent)
            .Select(z => z.AgentId)
            .Distinct()
            .ToArray();
    }

    private async Task PopulatePrivilegesList()
    {
        var lastTimeRefreshed = cachingService.GetLastPrivilegesRefreshTime();
        var json = cachingService.GetItem(nameof(EntityPrivilegeItem));

        if (lastTimeRefreshed != null && DateTime.UtcNow.Subtract(lastTimeRefreshed.Value)
               .TotalMinutes < 10 && string.IsNullOrEmpty(json) == false)
        {
            var obj = UtilityFunctions.DeserializeJsonString<EntityPrivilegeItem[]>(json);
            _entityPrivileges = obj;
        }

        if (_entityPrivileges.Length > 0)
        {
            return;
        }

        _entityPrivileges = await privilegesService.GetAllEntityPrivilegesAsync();
        cachingService.SaveLastPrivilegesRefreshTime();
        cachingService.SaveItem(nameof(EntityPrivilegeItem), UtilityFunctions.SerializeToJsonString(_entityPrivileges));
    }

    protected override void PopulateInitialData()
    {
    }

    private int GetAllowedFailedPassword()
    {
        return 5;
    }
}