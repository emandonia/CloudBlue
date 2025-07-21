using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Domain.Interfaces.DbContext;

public interface IUsersSessionsDataContext
{
    DbSet<UserSession> UserSessions { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync();

    Task SaveBulkChangesAsync();
}