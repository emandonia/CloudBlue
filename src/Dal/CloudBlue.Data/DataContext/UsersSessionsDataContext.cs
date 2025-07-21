using CloudBlue.Data.Configurations.Users;
using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.DataContext;

public class UsersSessionsDataContext : DbContext, IUsersSessionsDataContext

{
    public UsersSessionsDataContext()
    {
    }

    public async Task SaveBulkChangesAsync()
    {
        await this.BulkSaveChangesAsync();
    }
    public UsersSessionsDataContext(DbContextOptions<UsersSessionsDataContext> options) : base(options)
    {
    }

    public DbSet<UserSession> UserSessions { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserSessionConfiguration());
    }
}