using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices;

public sealed class PrivilegesService(
    IPrivilegesRepository repo,
    LoggedInUserInfo loggedInUserInfo) : BaseService, IPrivilegesService




{
    protected override void PopulateInitialData()
    {

    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }

    public async Task<ListResult<EntityPrivilegeItemForList>> GetEntityPrivilegesAsync(EntityPrivilegesFiltersModel filters)
    {
        return await repo.GetEntityPrivilegesAsync(filters);
    }
    public async Task<EntityPrivilegeItem[]> GetAllEntityPrivilegesAsync()
    {
        return await repo.GetAllEntityPrivilegesAsync
            ();
    }

    public async Task<bool> CreateEntityPrivilegeAsync(EntityPrivilegeModel model)
    {
        var existing = await repo.IsPrivilegeExistingAsync(model);

        if (existing)
        {
            return false;
        }

        return await repo.CreatePrivilegeAsync(model);
    }

    public async Task<bool> DeletePrivilegeAsync(long id)
    {
        return await repo.DeletePrivilegeAsync(id);
    }
}