using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.Crm;

public sealed class CallsService(
    ICallsRepository repo,
    IClientsService clientsService,
    ILeadTicketsService leadTicketsService,
    LoggedInUserInfo loggedInUserInfo,
    ISystemEventsService systemEventsService,
    ICallAllowedActionChecker allowedActionChecker) : BaseService, ICallsService
{
    public async Task<ListResult<CallItemForList>> GetCallsAsync(CallsFiltersModel callsFilters)
    {
        var privilege = CheckPrivilege(SystemPrivileges.CallsManage);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return new ListResult<CallItemForList>();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            callsFilters.BranchId = loggedInUserInfo.BranchId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            callsFilters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            callsFilters.CreatedById = loggedInUserInfo.UserId;
        }
        else if (privilege.PrivilegeScope != PrivilegeScopes.Global)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return new ListResult<CallItemForList>();
        }

        return await repo.GetCalls(callsFilters);
    }

    public async Task<bool> CreateCallAsync(CallCreateModel callCreateModel)
    {
        var privilege = CheckPrivilege(SystemPrivileges.CallsAdd);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            callCreateModel.BranchId = loggedInUserInfo.BranchId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            callCreateModel.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope != PrivilegeScopes.Global)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        if (callCreateModel.ClientId == 0)
        {
            var res = await CreateClientAsync(callCreateModel);

            if (res == false)
            {
                return false;
            }
        }
        else
        {
            await UpdateClientAsync(callCreateModel);
        }

        await CreateClientDevicesAsync(callCreateModel);
        callCreateModel.CallStatusId = (int)CallStatuses.New;

        if (callCreateModel is { CallTypeId: (int)CallTypes.Brokerage, LeadTicketModel.AgentId: > 0 })
        {
            callCreateModel.CallStatusId = (int)CallStatuses.Handled;
        }

        var result = await repo.CreateCallAsync(callCreateModel);

        if (result == false)
        {
            //todo: rollback client and devices creation
            LastErrors.Add(Errors.ErrorCreatingCall);

            return false;
        }

        callCreateModel.CallId = repo.LastCreatedItemId;
        await systemEventsService.GenerateNewCallEventsAsync(callCreateModel);

        if (callCreateModel.CallTypeId != (int)CallTypes.Brokerage &&
           callCreateModel.CallTypeId != (int)CallTypes.ALreadyExist)
        {
            return result;
        }

        result = await leadTicketsService.CreateLeadTicketFromCallAsync(callCreateModel);

        if (result == false)
        {
            //todo : rollback call, client and devices creation
            LastErrors.Add(Errors.ErrorCreatingLeadTicket);

            return false;
        }

        return true;
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    protected override void PopulateInitialData()
    {
        if (repo.CurrentUserId > 0)
        {
            return;
        }

        repo.CurrentUserBranchId = loggedInUserInfo.BranchId;
        repo.CurrentUserCompanyId = loggedInUserInfo.CompanyId;
        repo.CurrentUserId = loggedInUserInfo.UserId;
    }

    private async Task UpdateClientAsync(CallCreateModel callCreateModel)
    {
        await clientsService.UpdateClientAsync(callCreateModel.ClientInfo);
    }

    private async Task CreateClientDevicesAsync(CallCreateModel callCreateModel)
    {
        var newDevices = callCreateModel.ClientInfo.ClientContactDevices.Where(z => z.IsNew)
            .ToList();

        if (newDevices.Count > 0)
        {
            await clientsService.AddClientDevicesAsync(callCreateModel.ClientId, newDevices);
        }
    }

    private async Task<bool> CreateClientAsync(CallCreateModel callCreateModel)
    {
        var res = await clientsService.CreateClientAsync(callCreateModel.ClientInfo);

        if (res == false || clientsService.CreateItemId <= 0)
        {
            LastErrors.Add(Errors.ErrorCreatingClient);

            return false;
        }

        callCreateModel.ClientId = clientsService.CreateItemId;

        return true;
    }
}