using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CloudBlue.Data.Repositories;

public class UsersRepository(IUsersDataContext dbContext) : IUsersRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public async Task<ListResult<UserItemForList>> GetUsersAsync(UsersFiltersModel filters)
    {
        var retObj = new ListResult<UserItemForList>();

        var query = dbContext.VwUsers.Include(z => z.UserPhones)
            .AsQueryable();

        if (filters.Id > 0)
        {
            query = query.Where(z => z.Id == filters.Id);
        }

        if (filters.CompanyId > 0)
        {
            query = query.Where(z => z.CompanyId == filters.CompanyId);
        }

        if (filters.BranchId > 0)
        {
            query = query.Where(z => z.BranchId == filters.BranchId);
        }

        if (filters.TopManagerId > 0)
        {
            query = query.Where(z => z.TopMostManagerId == filters.TopManagerId);
        }

        if (filters.DepartmentId > 0)
        {
            query = query.Where(z => z.DepartmentId == filters.DepartmentId);
        }

        if (filters.PositionId > 0)
        {
            query = query.Where(z => z.PositionId == filters.PositionId);
        }

        if (filters.ApprovedStatus > 0)
        {
            var status = filters.ApprovedStatus == 1;
            query = query.Where(z => z.IsApproved == status);
        }

        if (filters.LockedStatus > 0)
        {
            var status = filters.LockedStatus == 1;
            query = query.Where(z => z.IsLockedOut == status);
        }

        if (string.IsNullOrEmpty(filters.Email) == false)
        {
            query = query.Where(z => EF.Functions.Like(z.Email, $"%{filters.Email.ToLower()}%"));


        }

        if (string.IsNullOrEmpty(filters.DeviceInfo) == false)
        {
            query = query.Where(z => z.UserPhones.Any(z => z.DeviceInfo == filters.DeviceInfo));
        }

        if (string.IsNullOrEmpty(filters.Username) == false)
        {
            query = query.Where(z => EF.Functions.Like(z.UserName, $"%{filters.Username.ToLower()}%"));
        }

        if (string.IsNullOrEmpty(filters.Fullname) == false)
        {
            query = query.Where(z => EF.Functions.Like(z.FullNameLowered, $"%{filters.Fullname.ToLower()}%"));
        }

        retObj.TotalCount = await query.CountAsync();

        var rawItems = await query.OrderBy(GetOrderBy(filters))
            .Skip(filters.PageIndex * filters.PageSize)
            .Take(filters.PageSize)
            .Select(z => new
            {
                z.CompanyId,
                z.BranchId,
                z.PositionId,
                z.BranchName,
                z.CompanyName,
                z.CreateDate,
                z.DepartmentName,
                z.DepartmentId,
                z.DirectManagerId,
                z.DirectManagerName,
                z.Email,
                z.FullName,
                z.HireDate,
                z.Id,
                z.UserName,
                z.IsLockedOut,
                z.TopMostManagerId,
                z.IsApproved,
                z.InResaleTeam,
                z.IsVirtual,
                z.LastLockoutDate,
                z.LastLoginDate,
                z.LastPasswordFailureDate,
                z.LastPromotionDate,
                z.ResignDate,
                z.TopMostManagerName,
                z.UserPositionName,
                z.UserPhones,
                z.CanAccessCommissionSystem,
                z.CanUserAccessPortal
            })
            .ToArrayAsync();

        var items = new UserItemForList[rawItems.Length];

        for (var i = 0; i < rawItems.Length; i++)
        {
            var z = rawItems[i];

            var item = new UserItemForList
            {
                CompanyId = z.CompanyId,
                BranchId = z.BranchId,
                PositionId = z.PositionId,
                BranchName = z.BranchName,
                CompanyName = z.CompanyName,
                CreateDate = z.CreateDate,
                DepartmentName = z.DepartmentName,
                DepartmentId = z.DepartmentId,
                DirectManagerId = z.DirectManagerId,
                DirectManagerName = z.DirectManagerName,
                Email = z.Email,
                FullName = z.FullName,
                HireDate = z.HireDate,
                Id = z.Id,
                UserName = z.UserName,
                IsLockedOut = z.IsLockedOut,
                TopMostManagerId = z.TopMostManagerId,
                IsApproved = z.IsApproved,
                InResaleTeam = z.InResaleTeam,
                IsVirtual = z.IsVirtual,
                LastLockoutDate = z.LastLockoutDate,
                LastLoginDate = z.LastLoginDate,
                LastPasswordFailureDate = z.LastPasswordFailureDate,
                LastPromotionDate = z.LastPromotionDate,
                ResignDate = z.ResignDate,
                TopMostManagerName = z.TopMostManagerName,
                UserPositionName = z.UserPositionName,
                CanAccessCommissionSystem = z.CanAccessCommissionSystem,
                CanUserAccessPortal = z.CanUserAccessPortal,

            };

            var defaultPhone = z.UserPhones.FirstOrDefault(z => z.IsDefault);

            if (defaultPhone != null)
            {
                item.AreaCode = defaultPhone.PhoneAreaCode;
                item.Phone = defaultPhone.Phone;
                item.DeviceInfo = defaultPhone.DeviceInfo;
                item.DeviceId = defaultPhone.Id;
            }

            items[i] = item;
        }

        retObj.Items = items;

        return retObj;
    }

    public async Task<bool> UpdateAccountStatusAsync(int userId, bool status)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        if (status)
        {
            user.ResignDate = null;
            user.ResignDateNumeric = 0;
        }

        user.IsApproved = status;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UnblockAccountAsync(int userId)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.IsLockedOut = false;
        user.FailedPasswordAttemptCount = 0;
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateUserPositionAsync(int userId, int positionId)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        var position = await dbContext.UserPositions.FirstOrDefaultAsync(z => z.Id == positionId);

        if (position == null)
        {
            return false;
        }

        user.PositionId = position.Id;
        user.DepartmentId = position.DepartmentId;
        user.UserGroupId = position.UserGroupId;
        user.LastPromotionDate = DateTime.UtcNow;
        user.LastPromotionDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateDirectManagerAsync(int userId, int directManagerId, string? directManagerName,
        int topManagerId, string? topManagerName, int[] managersArray)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(z => z.Id == userId);

        if (user == null)
        {
            return false;
        }

        user.DirectManagerId = directManagerId;
        user.DirectManagerName = directManagerName;
        user.TopMostManagerId = topManagerId;
        user.TopMostManagerName = topManagerName;
        user.ManagersIdsArray = managersArray;

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateUserSalesTreeAsync(int userId, string userFullName, int directManagerId)
    {
        var userTreeRecords = await dbContext.SalesUserTrees.Where(z => z.UserId == userId)
            .ToArrayAsync();

        if (userTreeRecords.Length == 0)
        {
            dbContext.SalesUserTrees.Add(new SalesUserTree
            {
                ParentId = directManagerId,
                UserFullName = userFullName,
                UserId = userId
            });

            await dbContext.SaveChangesAsync();

            return true;
        }

        var item = userTreeRecords.OrderByDescending(z => z.Id)
            .First();

        item.ParentId = directManagerId;

        if (userTreeRecords.Length > 1)
        {
            dbContext.SalesUserTrees.RemoveRange(userTreeRecords.Where(z => z.Id != item.Id)
                .ToArray());
        }

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User?> GetUserEntityAsync(int userId)
    {
        return await dbContext.Users.Include(z => z.UserPhones).FirstOrDefaultAsync(z => z.Id == userId);
    }

    public async Task<SalesPromotion[]> GetUserPromotionEntitiesAsync(int userId, bool onlyActive)
    {
        return await dbContext.SalesPromotions.Where(z =>
                z.SalesPersonId == userId && (onlyActive == false || z.PromotionEndDate == null))
            .ToArrayAsync();
    }

    public async Task<bool> CreateUserPromotionRecordAsync(int userId, int positionId, DateTime promotionDate)
    {
        dbContext.SalesPromotions.Add(new SalesPromotion
        {
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            PositionId = positionId,
            PromotionEndDate = null,
            PromotionEndDateNumeric = 0,
            PromotionStartDate = promotionDate,
            PromotionStartDateNumeric = int.Parse(promotionDate.ToString("yyyyMMdd")),
            SalesPersonId = userId
        });

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EndUserPromotionRecordsAsync(int userId, DateTime promotionEndDate)
    {
        var items = await dbContext.SalesPromotions
            .Where(z => z.SalesPersonId == userId && z.PromotionEndDateNumeric == 0)
            .ToArrayAsync();

        if (items.Length == 0)
        {
            return true;
        }

        foreach (var item in items)
        {
            item.PromotionEndDate = promotionEndDate;
            item.PromotionStartDateNumeric = int.Parse(promotionEndDate.ToString("yyyyMMdd"));
            item.LastUpdated = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<UserTreeItem[]> GetSalesUserTreeItemsAsync()
    {
        return await dbContext.VwSalesUserTrees.AsNoTracking()
            .Select(z => new UserTreeItem
            {
                UserId = z.UserId,
                AgentName = z.FullName,
                PositionId = z.PositionId,
                ParentId = z.ParentId,
                ApplyTargets = z.ApplyTargets,
                BranchId = z.BranchId,
                CanHaveTeam = z.CanHaveTeam,
                CompanyId = z.CompanyId,
                DirectManagerId = z.DirectManagerId,
                HireDateNumeric = z.HireDateNumeric,
                InResaleTeam = z.InResaleTeam,
                IsManager = z.IsManager,
                ResignDateNumeric = z.ResignDateNumeric,
                TopMostManagerId = z.TopMostManagerId
            })
            .ToArrayAsync();
    }

    public async Task<bool> IsUserInDepartmentAsync(int userId, int departmentId)
    {
        return await dbContext.Users.CountAsync(z => z.Id == userId && z.DepartmentId == departmentId) > 0;
    }

    public async Task<bool> SetResignedAsync(int userId, DateTime resignDate)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.ResignDate = UtilityFunctions.GetUtcDateTime(resignDate);
        user.ResignDateNumeric = int.Parse(user.ResignDate.Value.ToString("yyyyMMdd"));
        user.IsApproved = false;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<UserExistingResult[]> CheckUserExistAsync(string username, string email, string mobile)
    {
        var lst = await dbContext.Users.Include(u => u.UserPhones)
            .Where(z => (email != "" && z.Email == email) || (username != "" && z.UserName == username) ||
                        (mobile != "" && z.UserPhones.Any(m => m.DeviceInfo == mobile)))
            .Select(z => new UserExistingResult
            {
                Email = z.Email,
                Username = z.UserName,
                UserId = z.Id,
                Mobile = z.UserPhones.FirstOrDefault(p => p.DeviceInfo == mobile)
                    .DeviceInfo
            })
            .ToArrayAsync();

        return lst;
    }

    public async Task<bool> CreateUserAsync(CreateUserFullModel model)
    {
        var user = new User
        {
            BranchId = model.BranchId,
            CompanyId = model.CompanyId,
            CreateDate = DateTime.UtcNow,
            HireDate = model.HireDate,
            DepartmentId = model.DepartmentId,
            DirectManagerId = model.DirectManagerId,
            DirectManagerName = model.DirectManagerName,
            Email = model.Email,
            FailedPasswordAttemptCount = 0,
            IsApproved = true,
            TopMostManagerId = model.TopMostManagerId,
            FullName = model.FullName,
            FullNameLowered = model.FullNameLowered,
            InResaleTeam = model.InResaleTeam,
            IsBranchManager = false,
            IsLockedOut = false,
            IsParent = false,
            HireDateNumeric = model.HireDateNumeric,
            IsVirtual = false,
            LastLockoutDate = null,
            LastLoginDate = null,
            LastPasswordChangedDate = null,
            LastPasswordFailureDate = null,
            LastPromotionDate = model.HireDate,
            LastPromotionDateNumeric = model.HireDateNumeric,
            LastUpdateDate = null,
            ParentId = 0,
            Password = model.Password,
            PositionId = model.PositionId,
            PasswordSalt = model.PasswordSalt,
            ResetPasswordKey = null,
            ResignDate = null,
            ResignDateNumeric = 0,
            TeamsIds = [],
            CanAccessCommissionSystem = model.CanAccessCommissionSystem == 1,
            CanUserAccessPortal = model.CanUserAccessPortal == 1,
            TopMostManagerName = model.TopMostManagerName,
            UserName = model.Username,
            ManagersIdsArray = model.ManagersArray
        };

        var position = await dbContext.UserPositions.FirstOrDefaultAsync(z => z.Id == model.PositionId);

        if (position != null)
        {
            user.UserGroupId = position.UserGroupId;
        }

        user.UserPhones.Add(new UserPhone
        {
            DeviceInfo = model.UserPhone.DeviceInfo,
            DeviceTypeId = model.UserPhone.DeviceTypeId,
            IsDefault = true,
            Phone = model.UserPhone.Phone,
            PhoneAreaCode = model.UserPhone.AreaCode,
            PhoneCountryCode = model.UserPhone.CountryCode
        });

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        if (position != null && user.ManagersIdsArray != null && (position.IsManager || position.CanHaveTeam))
        {
            var lst = user.ManagersIdsArray.ToList();
            lst.Add(user.Id);
            user.ManagersIdsArray = lst.ToArray();
            await dbContext.SaveChangesAsync();
        }
        LastCreatedItemId = user.Id;

        return true;
    }

    public async Task<UserPosition[]> GetSalesUserPositionsEntitiesAsync()
    {
        var salesDepartmentId = 1;

        return await dbContext.UserPositions.Where(z => z.DepartmentId == salesDepartmentId)
            .ToArrayAsync();
    }

    public async Task<bool> UpdateFullNameAsync(int userId, string fullName)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.FullName = fullName;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateEmailAsync(int userId, string email)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.Email = email;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateUserMobileAsync(int userId, ClientPhoneModel userMobile)
    {
        var userPhone = await dbContext.UserPhones.FirstOrDefaultAsync(z => z.Id == userMobile.Id);

        if (userPhone == null)
        {
            return false;
        }

        userPhone.Phone = userMobile.Phone;
        userPhone.PhoneAreaCode = userMobile.AreaCode;
        userPhone.DeviceInfo = userMobile.DeviceInfo;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateUsernameAsync(int userId, string userName)
    {
        var user = await GetUserEntityAsync(userId);

        if (user == null)
        {
            return false;
        }

        user.UserName = userName;
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User[]> GetSalesUsersEntitiesAsync()
    {
        return await dbContext.Users.Where(z => z.DepartmentId == 1)
           .ToArrayAsync();
    }

    public async Task UpdateUsersAsync(User[] users)
    {
        dbContext.Users.UpdateRange(users);
        await dbContext.SaveBulkChangesAsync();
    }

    public async Task<bool> ChangePasswordAsync(int userId, string password, string passwordSalt)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(z => z.Id == userId);

        if (user == null)
        {
            return false;
        }

        user.Password = password;
        user.PasswordSalt = passwordSalt;
        user.LastPasswordChangedDate = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        return true;
    }

    private string GetOrderBy(SearchPager filters)
    {
        if (string.IsNullOrEmpty(filters.SortField))
        {
            return "Id desc";
        }

        return $"{filters.SortField} {filters.SortDirection}";
    }
}