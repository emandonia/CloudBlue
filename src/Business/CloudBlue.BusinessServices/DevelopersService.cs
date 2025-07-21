using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices;

public class DevelopersService(
    ILookUpsService lookUpsService,
    IDevelopersRepository repo,
    LoggedInUserInfo loggedInUserInfo) : BaseService, IDevelopersService

{
    public async Task<ConstructionDeveloperProject[]> GetProjectsEntitiesAsync(int[] projectIds)
    {
        return await repo.GetProjectsEntitiesAsync(projectIds);
    }

    protected override void PopulateInitialData()
    {
        //        throw new NotImplementedException();
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        throw new NotImplementedException();
    }
}