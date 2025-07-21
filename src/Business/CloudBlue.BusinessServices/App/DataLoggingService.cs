using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.App;

public class DataLoggingService(IAppDataContext db, LoggedInUserInfo loggedInUserInfo) : IDataLoggingService
{
    public async Task AddDataLog(BusinessActions action, CbPages page, string metaData)
    {
        var impersonated = false;
        var origUserId = 0;

        db.DataLogs.Add(new DataLog
        {
            Action = action,
            ActionDate = DateTime.UtcNow,
            MetaData = metaData,
            PageName = page.ToString(),
            UserId = loggedInUserInfo.UserId,
            UserName = loggedInUserInfo.UserName,
            ActionDateNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
            IsNew = true,
            Impersonated = impersonated,
            OriginalUserId = origUserId
        });

        await db.SaveChangesAsync();
    }
}