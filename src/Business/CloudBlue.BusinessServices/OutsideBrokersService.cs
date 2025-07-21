using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices;

public sealed class OutsideBrokersService(
    ILookUpsService lookUpsService,
    IOutsideBrokersRepository repo,
    LoggedInUserInfo loggedInUserInfo) : BaseService, IOutsideBrokersService

{
    protected override void PopulateInitialData()
    {
        throw new NotImplementedException();









    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        throw new NotImplementedException();
    }
}