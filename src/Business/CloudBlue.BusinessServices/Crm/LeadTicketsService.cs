using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.Crm;

public sealed class LeadTicketsService(
    ILookUpsService lookUpsService,
    ILeadTicketsRepository repo,
    LoggedInUserInfo loggedInUserInfo,
    ISystemEventsService systemEventsService,
    IClientsService clientsService,
    INotificationsService notificationsService,
    ILeadTicketAllowedActionChecker allowedActionChecker) : BaseService, ILeadTicketsService
{
    public async Task<bool> CreateLeadTicketAsync(LeadTicketCreateModel model)
    {
        #region Authorization

        var privilege = CheckPrivilege(SystemPrivileges.LeadTicketsAdd);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        if (CanAddLeadTicket(privilege, model) == false)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        #endregion

        if (model.ClientId == 0)
        {
            var res = await CreateClientAsync(model);

            if (res == false)
            {
                return false;
            }
        }
        else
        {
            await UpdateClientAsync(model);
        }

        await CreateClientDevicesAsync(model);
        var leadStatus = LeadTicketStatuses.Unassigned;

        if (model.AgentId > 0)
        {
            leadStatus = LeadTicketStatuses.Assigned;
        }

        model.LastAssignedDate = model.AgentId > 0 ? DateTime.UtcNow : null;
        model.LeadTicketStatusId = (int)leadStatus;
        model.IsFullLeadTicket = true;
        var result = await repo.CreateLeadTicketAsync(model);
        model.LeadTicketId = repo.LastCreatedItemId;

        if (result)
        {
            await systemEventsService.GenerateNewLeadTicketEventsAsync(model);
            await notificationsService.SendAssignedLeadTicketNotificationAsync(new List<LeadTicketInfoForEmail> { new LeadTicketInfoForEmail
            {
                AgentId = model.AgentId,
                ClientName = model.ClientInfo.ClientName,
                ClientPhone = model.ClientInfo.ClientContactDevices.First(z => z.DeviceTypeId!= (int)DeviceTypes.Email).DeviceInfo,
                PropertyTypeId=model.PropertyTypeId,
                ServiceId = model.ServiceId,
                ClientId = model.ClientId,
                Id = CreateItemId,
                District = model.Location.ProjectName



            } });
        }

        return result;
    }

    public async Task<bool> CreateLeadTicketFromCallAsync(CallCreateModel call)
    {
        #region Authorization

        var privilege = CheckPrivilege(SystemPrivileges.LeadTicketsAdd);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            call.BranchId = loggedInUserInfo.BranchId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            call.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope != PrivilegeScopes.Global)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        #endregion

        var leadStatus = call.LeadTicketModel.AgentId > 0 ? LeadTicketStatuses.Assigned : LeadTicketStatuses.Unassigned;

        var model = new LeadTicketCreateModel
        {
            LeadSourceInfo = call.LeadSourceInfo,
            ClientId = call.ClientId,
            ReferralId = 0,
            AgentName = call.AgentName,
            SalesTypeId = call.LeadTicketModel.SalesTypeId,
            AgentId = call.LeadTicketModel.AgentId,
            CallId = call.CallId,
            CurrentUserName = call.CurrentUserName,
            LastAssignedDate = call.LeadTicketModel.AgentId > 0 ? DateTime.UtcNow : null,
            CorporateCompanyId = 0,
            UsageId = call.LeadTicketModel.UsageId,
            ApplyCampaignOwnerShipRules = false,
            ServiceId = call.LeadTicketModel.ServiceId,
            PropertyTypeId = call.LeadTicketModel.PropertyTypeId,
            ClientBudget = call.LeadTicketModel.ClientBudget,
            BranchId = call.BranchId,
            BranchName = call.BranchName,
            Comment = call.CallComment,
            CompanyId = call.CompanyId,
            CompanyName = call.CompanyName,
            LeadTicketStatusId = (int)leadStatus,
            ApplyTwentyFourHoursRules = false,
            Location = call.Location,
            LocationStr = call.LocationStr,
            IsFullLeadTicket = call.CallTypeId == (int)CallTypes.Brokerage,
            PendingAlreadyExistView = call.CallTypeId == (int)CallTypes.ALreadyExist
        };

        var result = await repo.CreateLeadTicketAsync(model);
        CreateItemId = repo.LastCreatedItemId;
        call.LeadTicketModel.LeadTicketId = CreateItemId;

        if (result)
        {
            await systemEventsService.GenerateNewLeadTicketEventsAsync(model);
            await notificationsService.SendAssignedLeadTicketNotificationAsync(new List<LeadTicketInfoForEmail> { new LeadTicketInfoForEmail
            {
                AgentId = model.AgentId,
                ClientName = model.ClientInfo.ClientName,
                ClientPhone = model.ClientInfo.ClientContactDevices.First(z => z.DeviceTypeId!= (int)DeviceTypes.Email).DeviceInfo,
                PropertyTypeId=model.PropertyTypeId,
                ServiceId = model.ServiceId,
                ClientId = model.ClientId,
                Id = CreateItemId,
                District = model.Location.ProjectName


            } });
        }

        return result;
    }

    public async Task<ListResult<LeadTicketItemForList>> GetLeadTicketsAsync(LeadTicketsFiltersModel filters)
    {
        #region Authorization

        var privilege = CheckPrivilege(SystemPrivileges.LeadTicketsManage);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return new ListResult<LeadTicketItemForList>();
        }

        var populateUsers = false;

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            filters.BranchId = loggedInUserInfo.BranchId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam ||
                privilege.PrivilegeScope == PrivilegeScopes.TreeTeam || privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            populateUsers = true;
            //filters.BranchId = loggedInUserInfo.BranchId;
            //filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope != PrivilegeScopes.Global)
        {
            return new ListResult<LeadTicketItemForList> { Items = [] };
        }

        #endregion

        if (filters.AgentIdsList.Any() || populateUsers || loggedInUserInfo.TeamsIds.Length > 0)
        {
            await PopulateSalesPersonsLists(filters, privilege, populateUsers);
        }

        if (filters.ModeId > 0)
        {
            AppLyModeFilters(filters);
        }

        var retObj = await repo.GetLeadTicketsAsync(filters, lookUpsService);

        if (retObj.Items.Length > 0 && filters.ExportMode == false)
        {
            allowedActionChecker.PopulateAllowedActions(retObj.Items);
        }

        return retObj;
    }

    private void AppLyModeFilters(LeadTicketsFiltersModel filters)
    {
        ManagePagesModes mode;
        if (Enum.IsDefined(typeof(ManagePagesModes), filters.ModeId) == false)
        {
            return;
        }

        mode = (ManagePagesModes)filters.ModeId;

        switch (mode)
        {
            case ManagePagesModes.CallLaterLeads:

                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.Assigned, (int)LeadTicketStatuses.InProgress };
                filters.PendingActivityId = (int)ContactingTypes.CallLater;
                break;
            case ManagePagesModes.FreshLeads:
                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.Assigned };
                filters.ExtremeHours = 2;
                filters.AssigningType = AssigningTypes.AllFreshLeads;
                filters.ReverseAssignDateComparison = true;
                break;
            case ManagePagesModes.NewLeadsExceedTwoHours:
                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.Assigned };
                filters.ExtremeHours = 2;
                filters.ReverseAssignDateComparison = false;
                break;


            case ManagePagesModes.NewLeadsReassigned:
                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.Assigned };
                filters.AssigningType = AssigningTypes.ReAssigned;

                break;

            case ManagePagesModes.NoAnswerLeads:
                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.Assigned, (int)LeadTicketStatuses.InProgress };
                filters.LastFeedbackOnly = true;
                filters.FeedbackIds = new List<int> { (int)ContactingTypes.NoAnswer };
                break;

            case ManagePagesModes.QualifiedLeadsExceedTwoWeeks:
                filters.EntityStatusIds = new List<int> { (int)LeadTicketStatuses.InProgress };

                filters.EntityAssignDateTo = DateTime.UtcNow.AddDays(-14);

                break;


        }
    }

    public async Task<LeadTicketInfoItemForTcr?> GetLeadTicketForPrimeTcrAsync(long id)
    {
        var privilege = CheckPrivilege(SystemPrivileges.LeadTicketsConvertToPrimeTcr);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return null;
        }

        var leadTicket = await repo.GetLeadTicketForPrimeTcrAsync(id);

        if (leadTicket == null)
        {
            LastErrors.Add(Errors.ItemNotFound);

            return null;
        }

        var itemToCheck = new LeadTicketItemForList
        {
            BranchId = leadTicket.BranchId,
            CompanyId = leadTicket.CompanyId,
            CurrentAgentId = leadTicket.AgentId,
            LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId,
            ServiceId = (LeadTicketServices)leadTicket.ServiceTypeId,
            SalesTypeId = (SalesTypes)leadTicket.SalesTypeId
        };

        var result = allowedActionChecker.CanConvertToPrimeTcr(itemToCheck);

        if (result == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return null;
        }

        var tcrExist = await repo.CheckTcrExistsAsync(id);

        if (tcrExist)
        {
            LastErrors.Add(Errors.APrimeTcrIsAssociatedWithThisLeadTicketId);

            return null;
        }

        return leadTicket;
    }

    public async Task UpdateLeadTicketTcrStatusAsync(List<long> leadTicketIds, int status, EntityTypes tcrType)
    {
        var leads = await repo.GetLeadTicketEntitiesAsync(leadTicketIds, true, true);

        if (leads.Count == 0)
        {
            return;
        }

        for (var idx = 0; idx < leadTicketIds.Count; idx++)
        {
            var leadTicket = leads[idx];
            leadTicket.TcrStatusId = status;
            leadTicket.TcrTypeId = (int)tcrType;

            if (leadTicket.TcrStatusId != (int)LeadTicketStatuses.Tcr)
            {
                leadTicket.LeadTicketExtension.OldStatusId = leadTicket.LeadTicketStatusId;
                leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Tcr;

                await systemEventsService.GenerateLeadTicketsConvertedToTcrAsync(leadTicket.Id, tcrType,
                    leadTicket.ClientId);

            }
        }

        await repo.UpdateEntitiesAsync(leads);
    }

    public async Task UpdateLeadTicketViewedByAgentAsync(long id)
    {
        var leads = await repo.GetLeadTicketEntitiesAsync([id], true, true);

        if (leads.Count != 1)
        {
            return;
        }

        var lead = leads.First();
        lead.ViewedByCurAgent = true;
        await repo.UpdateEntitiesAsync(new List<LeadTicket>() { lead });
        var comment = $"Lead ticket was viewed by {loggedInUserInfo.FullName}";
        await systemEventsService.AddEventAsync(lead.Id, EntityTypes.LeadTicket, EventTypes.SystemGenerated, lead.ClientId, loggedInUserInfo.UserId, comment, EventProcesses.ViewLeadTicket, 0);
    }

    private bool CanAddLeadTicket(UserPrivilegeItem privilege, LeadTicketCreateModel model)
    {
        if (privilege.PrivilegeScope == PrivilegeScopes.Branch && model.BranchId == loggedInUserInfo.BranchId)
        {
            return true;
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Company && model.CompanyId == loggedInUserInfo.CompanyId)
        {
            return true;
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Own && model.AgentId == loggedInUserInfo.UserId)
        {
            return true;
        }

        if ((privilege.PrivilegeScope == PrivilegeScopes.DirectTeam ||
            privilege.PrivilegeScope == PrivilegeScopes.TreeTeam) &&
           (loggedInUserInfo.AgentsIds.Contains(model.AgentId) || loggedInUserInfo.MangersIds.Contains(model.AgentId)))
        {
            return true;
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Global)
        {
            return true;
        }

        return false;
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    private async Task CreateClientDevicesAsync(LeadTicketCreateModel model)
    {
        var newDevices = model.ClientInfo.ClientContactDevices.Where(z => z.IsNew)
            .ToList();

        if (newDevices.Count > 0)
        {
            await clientsService.AddClientDevicesAsync(model.ClientId, newDevices);
        }
    }

    private async Task UpdateClientAsync(LeadTicketCreateModel model)
    {
        await clientsService.UpdateClientAsync(model.ClientInfo);
    }

    private async Task<bool> CreateClientAsync(LeadTicketCreateModel model)
    {
        var res = await clientsService.CreateClientAsync(model.ClientInfo);

        if (res == false || clientsService.CreateItemId <= 0)
        {
            LastErrors.Add(Errors.ErrorCreatingClient);

            return false;
        }

        model.ClientId = model.ClientInfo.ClientId = clientsService.CreateItemId;

        return true;
    }

    private async Task PopulateSalesPersonsLists(LeadTicketsFiltersModel filters, UserPrivilegeItem privilege,
        bool populateUsers)
    {
        filters.AgentsIds.Clear();
        filters.ManagersIds.Clear();
        if (privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            filters.AgentsIds.Clear();
            filters.AgentsIds.Add(loggedInUserInfo.UserId);

            return;
        }

        var allActiveAgents = await lookUpsService.GetAgentsAsync(false);



        if (loggedInUserInfo.TeamsIds.Length > 0)
        {
            filters.ManagersIds.AddRange(loggedInUserInfo.TeamsIds);
        }

        if (filters.TopManagerId > 0)
        {
            filters.ManagersIds.Add(filters.TopManagerId);
        }
        if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam || privilege.PrivilegeScope == PrivilegeScopes.TreeTeam)
        {
            if (!filters.AgentIdsList.Any())
            {
                filters.ManagersIds.Add(loggedInUserInfo.UserId);
            }
            else
            {
                var currentUser = allActiveAgents.FirstOrDefault(z => z.AgentId == loggedInUserInfo.UserId);

                if (currentUser != null)
                {
                    if (currentUser.SalesPersonClass == SalesPersonClasses.TopManager)
                    {
                        filters.TopManagerId = loggedInUserInfo.UserId;
                    }
                    else
                    {
                        filters.DirectManagerId = loggedInUserInfo.UserId;
                    }
                }

            }

        }


        if (!filters.AgentIdsList.Any())
        {
            return;
        }



        var teamIds = allActiveAgents.Select(z => z.AgentId)
            .Distinct()
            .ToList();

        if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam)
        {
            if (filters.AgentIdsList.Any())
            {
                filters.AgentsIds = teamIds.Where(z => filters.AgentIdsList.Contains(z))
                    .ToList();
            }

            return;
        }




        var selectedAgents = allActiveAgents.Where(z => filters.AgentIdsList.Contains(z.AgentId))
            .ToArray();

        //  filters.ManagersIds.Clear();



        if (filters.AgentsRecursive == false)
        {
            filters.AgentsIds = selectedAgents.Any()
                ? selectedAgents.Select(z => z.AgentId)
                    .ToList()
                : allActiveAgents.Select(z => z.AgentId)
                    .ToList();

            return;
        }

        var hasFilters = selectedAgents.Any();

        if (hasFilters == false)
        {
            //  selectedAgents = allActiveAgents;
        }

        ClassifyAgents(selectedAgents, filters, hasFilters);
    }

    private void ClassifyAgents(IReadOnlyCollection<AgentItem> selectedAgents, LeadTicketsFiltersModel filters,
        bool hasFilters)
    {
        var managersIds = selectedAgents.Where(z => z.SalesPersonClass == SalesPersonClasses.TopManager || z.SalesPersonClass == SalesPersonClasses.Manager)
            .Select(z => z.AgentId)
            .Distinct()
            .ToList();

        filters.ManagersIds.AddRange(managersIds);


        if (hasFilters == false && managersIds.Any())
        {
            return;
        }






        var agentsIds = selectedAgents.Where(z =>
                z.SalesPersonClass == SalesPersonClasses.Agent &&
                managersIds.Contains(z.DirectManagerId) == false &&
                managersIds.Contains(z.TopMostManagerId) == false)
            .Select(z => z.AgentId)
            .Distinct()
            .ToList();

        filters.AgentsIds.AddRange(agentsIds);
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
}