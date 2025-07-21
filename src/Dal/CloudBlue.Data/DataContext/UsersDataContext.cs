using CloudBlue.Data.Configurations.Crm;
using CloudBlue.Data.Configurations.Users;
using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.DataContext;

public class UsersDataContext : DbContext, IUsersDataContext

{
    public UsersDataContext()
    {
    }

    public UsersDataContext(DbContextOptions<UsersDataContext> options) : base(options)
    {
    }

    public async Task SaveBulkChangesAsync()
    {
    await this.BulkSaveChangesAsync();
    }

    public DbSet<EntityPrivilege> EntityPrivileges { get; set; }
    public DbSet<VwUser> VwUsers { get; set; }
    public DbSet<VwSalesUserTree> VwSalesUserTrees { get; set; }
    public DbSet<VwEntityPrivilege> VwEntityPrivileges { get; set; }
    public DbSet<Privilege> Privileges { get; set; }
    public DbSet<PrivilegeCategory> PrivilegeCategories { get; set; }
    public DbSet<PrivilegeScope> PrivilegeScopes { get; set; }
    public DbSet<SalesPromotion> SalesPromotions { get; set; }
    public DbSet<PrivilegeEntityType> PrivilegeEntityTypes { get; set; }
    public DbSet<SalesUserTree> SalesUserTrees { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }

    public DbSet<UserPhone> UserPhones { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    public async Task<int> SaveChangesAsync()
    {
    return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    modelBuilder.ApplyConfiguration(new EntityPrivilegeConfiguration());
    modelBuilder.ApplyConfiguration(new PrivilegeEntityTypeConfiguration());
    modelBuilder.ApplyConfiguration(new PrivilegeCategoryConfiguration());
    modelBuilder.ApplyConfiguration(new VwEntityPrivilegeConfiguration());
    modelBuilder.ApplyConfiguration(new PrivilegeScopeConfiguration());
    modelBuilder.ApplyConfiguration(new PrivilegeConfiguration());
    modelBuilder.ApplyConfiguration(new VwCallRecipientConfiguration());
    modelBuilder.ApplyConfiguration(new VwAgentConfiguration());
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new UserSessionConfiguration());
    modelBuilder.ApplyConfiguration(new VwUserConfiguration());
    modelBuilder.ApplyConfiguration(new VwSalesUserTreeConfiguration());
    modelBuilder.ApplyConfiguration(new SalesPromotionConfiguration());
    }
}