using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.Repositories;

public class UsersSessionsRepository(IUsersSessionsDataContext db) : IUsersSessionsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

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