using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserAuth;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.Repositories;

public class UsersAuthRepository(IUsersDataContext db) : IUsersAuthRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public async Task<UserInfoForValidation?> GetUserByUsernameAsync(string username)
    {
        return await db.Users.Select(z => new UserInfoForValidation
        {
            IsApproved = z.IsApproved,
            IsLockedOut = z.IsLockedOut,
            UserName = z.UserName,
            Password = z.Password,
            PasswordSalt = z.PasswordSalt,
            Email = z.Email,
            FailedPasswordAttemptCount = z.FailedPasswordAttemptCount,
            FullName = z.FullName,
            CompanyId = z.CompanyId,
            BranchId = z.BranchId,
            DepartmentId = z.DepartmentId,
            ParentId = z.ParentId,
            IsParent = z.IsParent,
            IsVirtual = z.IsVirtual,
            PositionId = z.PositionId,
            Id = z.Id,
            CanAccessCommissionSystem = z.CanAccessCommissionSystem,
            CanUserAccessPortal = z.CanUserAccessPortal,
        })
            .FirstOrDefaultAsync(z => z.UserName == username || z.Email == username);
    }

    public Task<bool> ForgetPassword(LoginItem loginItem)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateFailedPasswordAttemptCount(int userId, int failedPasswordAttemptCount, bool lockedOut)
    {
        var user = await db.Users.FirstOrDefaultAsync(z => z.Id == userId);

        if (user != null)
        {
            user.FailedPasswordAttemptCount = failedPasswordAttemptCount;
            user.LastPasswordFailureDate = DateTime.UtcNow;

            if (lockedOut)
            {
                user.IsLockedOut = true;
            }

            db.Users.Update(user);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateSuccessLogin(int userInfoId)
    {
        var user = await db.Users.FirstOrDefaultAsync(z => z.Id == userInfoId);

        if (user == null)
        {
            return;
        }

        user.LastLoginDate = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

    public async Task<LoggedInUserInfo?> GetLoggedInUserInfo(int userInfoId)
    {
        var retObj = await db.VwUsers.Select(z => new LoggedInUserInfo
        {
            BranchId = z.BranchId,
            BranchName = z.BranchName,
            CompanyId = z.CompanyId,
            DepartmentId = z.DepartmentId,
            Email = z.Email,
            FullName = z.FullName,
            ParentId = z.ParentId,
            PositionId = z.PositionId,
            UserPositionName = z.UserPositionName,
            UserGroupId = z.UserGroupId,
            UserId = z.Id,
            UserName = z.UserName,
            CompanyName = z.CompanyName,
            IsParent = z.IsParent,
            CanHaveTeam = z.CanHaveTeam,
            DirectManagerId = z.DirectManagerId,
            CurrentAccessPrivilege = false,
            HasLineOfBusiness = z.HasLineOfBusiness,
            InResaleTeam = z.InResaleTeam,
            IsManager = z.IsManager,
            TopMostManagerId = z.TopMostManagerId,
            TeamsIds = z.TeamsIds
        })
            .FirstOrDefaultAsync(z => z.UserId == userInfoId);

        if (retObj == null)
        {
            return null;
        }

        if (retObj.IsParent == false)
        {
            return retObj;
        }

        retObj.SubAccounts = await db.VwUsers
            .Where(z => z.ParentId == userInfoId && z.IsApproved && z.IsLockedOut == false)
            .Select(z => new SubAccountItem
            {
                UserId = z.Id,
                UserFullName = z.FullName,
                UserPosition = z.UserPositionName
            })
            .ToArrayAsync();

        return retObj;
    }

    public async Task<bool> CreateUserSession(UserSessionItem userSession)
    {
        var session = new UserSession
        {
            DeviceServiceId = userSession.DeviceServiceId,
            UserId = userSession.UserId,
            SerializedObject = userSession.SerializedObject,
            ApiKey = userSession.ApiKey,
            BranchId = userSession.BranchId,
            CompanyId = userSession.CompanyId,
            CreationDate = DateTime.UtcNow,
            ExpireDate = userSession.ExpireDate,
            ExpireDateNumeric = long.Parse(userSession.ExpireDate.ToString("yyyyMMddHHmm")),
            IsExpired = false,
            LoginProvider = userSession.LoginProvider,
            SetExpiredOn = null
        };

        db.UserSessions.Add(session);
        var result = await db.SaveChangesAsync() > 0;
        userSession.Id = session.Id;

        return result;
    }

    public async Task<UserSessionItem?> GetActiveUserSessionAsync(string apiKey)
    {
        return await db.UserSessions.Where(z => z.IsExpired == false && z.ApiKey == apiKey)
            .Select(z => new UserSessionItem
            {
                Id = z.Id,
                UserId = z.UserId,
                DeviceServiceId = z.DeviceServiceId,
                ExpireDate = z.ExpireDate,
                BranchId = z.BranchId,
                CompanyId = z.CompanyId,
                LoginProvider = z.LoginProvider,
                SerializedObject = z.SerializedObject,
                ApiKey = z.ApiKey
            })
            .FirstOrDefaultAsync();
    }

    public async Task SetSessionExpiredAsync(long sessionId, string? apiKey = "")
    {
        var session = await db.UserSessions.FirstOrDefaultAsync(z =>
            sessionId == 0 || z.Id == sessionId || apiKey == "" || z.ApiKey == apiKey);

        if (session == null)
        {
            return;
        }

        session.IsExpired = true;
        session.SetExpiredOn = DateTime.UtcNow;
        await db.SaveChangesAsync();
    }

}