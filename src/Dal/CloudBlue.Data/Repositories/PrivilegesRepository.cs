using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CloudBlue.Data.Repositories;

public class PrivilegesRepository(IUsersDataContext appDb) : IPrivilegesRepository
{

    public async Task<ListResult<EntityPrivilegeItemForList>> GetEntityPrivilegesAsync(EntityPrivilegesFiltersModel filters)
    {
        var query = appDb.VwEntityPrivileges.AsQueryable();

        if (string.IsNullOrEmpty(filters.ExtraFilters) == false)
        {
            query = query.Where(filters.ExtraFilters);
        }
        if (filters.EntityId > 0)
        {
            query = query.Where(z => z.EntityId == filters.EntityId);
        }
        if (filters.EntityTypeId > 0)
        {
            var entityType = (PrivilegeEntityTypes)filters.EntityTypeId;
            query = query.Where(z => z.PrivilegeEntityType == entityType);
        }
        if (filters.PrivilegeId > 0)
        {
            var privilege = (SystemPrivileges)filters.PrivilegeId;
            query = query.Where(z => z.Privilege == privilege);
        }
        if (filters.PrivilegeScopeId > 0)
        {
            var privilegeScope = (PrivilegeScopes)filters.PrivilegeScopeId;
            query = query.Where(z => z.PrivilegeScope == privilegeScope);
        }
        if (filters.PrivilegeCategoryId > 0)
        {
            var privilegeCategory = (PrivilegeCategories)filters.PrivilegeCategoryId;
            query = query.Where(z => z.PrivilegeCategory == privilegeCategory);
        }
        var retObj = new ListResult<EntityPrivilegeItemForList>();
        retObj.TotalCount = await query.CountAsync();


        var orderBy = $"{filters.SortField ?? "Id"} {filters.SortDirection ?? "Desc"}";
        query = query.OrderBy(orderBy);
        query = query.Skip(filters.PageIndex * filters.PageSize).Take(filters.PageSize);

        var entityPrivileges = await
        query.Select(z => new EntityPrivilegeItemForList
        {
            PrivilegeScope = z.PrivilegeScope,

            PrivilegeMetaData = z.PrivilegeMetaData,

            Id = z.Id,
            EntityName = z.EntityName,
            PrivilegeCategoryName = z.PrivilegeCategoryName,
            PrivilegeEntityTypeName = z.PrivilegeEntityTypeName,
            PrivilegeName = z.PrivilegeName,
            PrivilegeScopeName = z.PrivilegeScopeName
        })
           .ToArrayAsync();



        retObj.Items = entityPrivileges;

        return retObj;
    }
    public async Task<EntityPrivilegeItem[]> GetAllEntityPrivilegesAsync()
    {

        var entityPrivileges = await
            appDb.VwEntityPrivileges.Select(z => new EntityPrivilegeItem
            {
                Privilege = z.Privilege,
                PrivilegeCategory = z.PrivilegeCategory,
                PrivilegeScope = z.PrivilegeScope,
                PrivilegeEntityType = z.PrivilegeEntityType,
                ActionName = z.ActionName,
                ControllerName = z.ControllerName,
                EntityId = z.EntityId,
                Path = z.Path,
                PrivilegeMetaData = z.PrivilegeMetaData,
                AccessOnly = z.AccessOnly,
                Id = z.Id
            })
                .ToArrayAsync();

        for (var i = 0; i < entityPrivileges.Length; i++)
        {
            entityPrivileges[i].PrivilegeScopeId = (int)entityPrivileges[i].PrivilegeScope;
            entityPrivileges[i].PrivilegeEntityTypeId = (int)entityPrivileges[i].PrivilegeEntityType;
        }

        return entityPrivileges;
    }

    public async Task<bool> IsPrivilegeExistingAsync(EntityPrivilegeModel model)
    {
        return await appDb.EntityPrivileges.CountAsync(z =>
            z.PrivilegeId == model.PrivilegeId && z.PrivilegeEntityTypeId == model.PrivilegeEntityTypeId &&
            z.EntityId == model.EntityId) > 0;
    }

    public async Task<bool> CreatePrivilegeAsync(EntityPrivilegeModel model)
    {
        await appDb.EntityPrivileges.AddAsync(new EntityPrivilege
        {
            EntityId = model.EntityId,
            PrivilegeScopeId = model.PrivilegeScopeId,
            EntityName = model.EntityName,
            PrivilegeEntityTypeId = model.PrivilegeEntityTypeId,
            PrivilegeId = model.PrivilegeId
        });

        await appDb.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePrivilegeAsync(long id)
    {
        var item = await appDb.EntityPrivileges.FirstOrDefaultAsync(z => z.Id == id);

        if (item == null)
        {
            return false;
        }

        appDb.EntityPrivileges.Remove(item);
        await appDb.SaveChangesAsync();

        return true;


    }

    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }
}