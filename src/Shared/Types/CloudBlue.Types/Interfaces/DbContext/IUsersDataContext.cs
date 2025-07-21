using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Operations;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Domain.Interfaces.DbContext;

public interface IUsersDataContext
{
    DbSet<EntityPrivilege> EntityPrivileges { get; set; }

    DbSet<VwUser> VwUsers { get; set; }
    DbSet<VwSalesUserTree> VwSalesUserTrees { get; set; }
    DbSet<VwEntityPrivilege> VwEntityPrivileges { get; set; }

    DbSet<Privilege> Privileges { get; set; }

    DbSet<PrivilegeCategory> PrivilegeCategories { get; set; }

    DbSet<PrivilegeScope> PrivilegeScopes { get; set; }

    DbSet<SalesPromotion> SalesPromotions { get; set; }
    DbSet<PrivilegeEntityType> PrivilegeEntityTypes { get; set; }

    DbSet<SalesUserTree> SalesUserTrees { get; set; }

    DbSet<User> Users { get; set; }

    DbSet<UserGroup> UserGroups { get; set; }

    DbSet<UserPhone> UserPhones { get; set; }

    DbSet<UserPosition> UserPositions { get; set; }

    DbSet<UserSession> UserSessions { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync();

    Task SaveBulkChangesAsync();
}