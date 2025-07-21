using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.PrimeTcrs;

public class PrimeTcrsService(
    ILookUpsService lookUpsService,
    IPrimeTcrsRepository repo,
    ILeadTicketsService leadTicketsService,
    LoggedInUserInfo loggedInUserInfo,
    ISystemEventsService systemEventsService,
    IUsersDataService usersDataService,
    IDevelopersService developersService,
    IPrimeTcrAllowedActionChecker allowedActionChecker) : BaseService, IPrimeTcrsService
{
    public async Task<bool> CreatePrimeTcrAsync(CreatePrimeTcrModel inputModel)
    {
        if (CanCreatePrimeTcr(inputModel.LeadTicketId) == false)
        {
            return false;
        }

        var model = await PopulateValidBasicData(inputModel);

        if (model == null)
        {
            return false;
        }

        var status = PrimeTcrStatuses.Reserved;

        if (model.IsRecContracted)
        {
            status = PrimeTcrStatuses.Contracted;
            model.RecCloseDate = model.CreationDateTime;
            model.RecCloseDateNumeric = model.CreationDateNumeric;
        }
        else
        {
            model.RecReserveDate = model.CreationDateTime;
            model.RecReserveDateNumeric = model.CreationDateNumeric;
        }

        model.PrimeTcrStatusId = (int)status;
        model.TaxPercentage = decimal.Parse("0.14");
        model.PendingStageId = model.PrimeTcrStatusId;
        await PopulatesAgentsTrees(model);
        var result = await repo.CreatePrimeTcrAsync(model);

        if (result == false)
        {
            return false;
        }

        model.PrimeTcrId = repo.LastCreatedItemId;
        await leadTicketsService.UpdateLeadTicketTcrStatusAsync(new List<long> { model.LeadTicketId }, (int)status, EntityTypes.PrimeTcr);

        await systemEventsService.GenerateNewPrimeTcrSystemEventsAsync(model.PrimeTcrId, model.ClientId,
            model.IsRecContracted, model.LeadTicketId);

        return true;
    }
    private void AppLyModeFilters(PrimeTcrsFiltersModel filters)
    {
        ManagePagesModes mode;
        if (Enum.IsDefined(typeof(ManagePagesModes), filters.ModeId) == false)
        {
            return;
        }

        mode = (ManagePagesModes)filters.ModeId;

        switch (mode)
        {
            case ManagePagesModes.ContractedDeals:

                filters.EntityStatusIds = new List<int> { (int)PrimeTcrStatuses.Contracted };
                break;
            case ManagePagesModes.ReOpenedDeals:
                filters.EntityStatusIds = new List<int> { (int)PrimeTcrStatuses.ReopenDev, (int)PrimeTcrStatuses.ReopenSales };
                break;
            case ManagePagesModes.ReservedDealsForOneMonth:
                filters.EntityStatusIds = new List<int> { (int)PrimeTcrStatuses.Reserved };
                filters.EntityCreationDateTo = DateTime.UtcNow.AddMonths(-2);

                break;




        }
    }

    public async Task<ListResult<PrimeTcrItemForList>> GetPrimeTcrsAsync(PrimeTcrsFiltersModel filters)
    {
        #region Authorization

        var privilege = CheckPrivilege(SystemPrivileges.PrimeTcrsManage);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return new ListResult<PrimeTcrItemForList>();
        }

        var populateUsers = false;

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            filters.BranchId = loggedInUserInfo.BranchId;
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam ||
                privilege.PrivilegeScope == PrivilegeScopes.TreeTeam || privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            populateUsers = true;
            filters.BranchId = loggedInUserInfo.BranchId;
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Global)
        {
            filters.UseTcrCompany = true;
        }
        else
        {
            return new ListResult<PrimeTcrItemForList> { Items = [] };

        }

        #endregion

        if (populateUsers || loggedInUserInfo.TeamsIds.Length > 0)
        {
            PopulateSalesPersonsLists(filters, privilege);
        }

        await SettingAgentSearchCriteria(filters);

        filters.UseTcrCompany =
            loggedInUserInfo.Privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.Accounting);

        if (filters.ModeId > 0)
        {
            AppLyModeFilters(filters);
        }
        var retObj = await repo.GetPrimeTcrsAsync(filters);

        if (retObj.Items.Length > 0 && filters.ExportMode == false)
        {
            allowedActionChecker.PopulateAllowedActions(retObj.Items);
        }

        return retObj;
    }

    public async Task<PrimeTcrFullItem?> GetPrimeTcrForViewAsync(long id)
    {

        var filters = new PrimeTcrItemFiltersModel { PrimeTcrId = id };

        #region Authorization

        var privilege = CheckPrivilege(SystemPrivileges.PrimeTcrsView);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return null;
        }

        var populateUsers = false;

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            filters.BranchId = loggedInUserInfo.BranchId;
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam ||
                privilege.PrivilegeScope == PrivilegeScopes.TreeTeam || privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            populateUsers = true;
            filters.BranchId = loggedInUserInfo.BranchId;
            filters.CompanyId = loggedInUserInfo.CompanyId;
        }
        else if (privilege.PrivilegeScope == PrivilegeScopes.Global)
        {
            filters.UseTcrCompany = true;
        }
        else
        {
            return null;
        }

        #endregion

        if (populateUsers || loggedInUserInfo.TeamsIds.Length > 0)
        {
            filters.ManagersIds.Clear();
            if (privilege.PrivilegeScope == PrivilegeScopes.Own)
            {

                filters.AgentId = loggedInUserInfo.UserId;

            }
            else if (loggedInUserInfo.TeamsIds.Length > 0)
            {
                filters.ManagersIds.AddRange(loggedInUserInfo.TeamsIds);
            }
            else
            {
                filters.ManagersIds.Add(loggedInUserInfo.UserId);

                if (filters.AgentId == 0)
                {
                    filters.AgentId = loggedInUserInfo.UserId;

                    filters.UserAgentOr = true;
                }
            }
        }




        /*
         *  if (userdata.IsSalesAccountant == false && userdata.IsSalesDirector == false)
           {
               whereStatement += " and (it.[EventProcessFK]<53 || it.EventProcessFK>57)";
           }

          
         */
        var primeTcr = await repo.GetSinglePrimeTcrAsync(filters);

        if (primeTcr == null)
        {
            LastErrors.Add(Errors.ItemNotFound);

            return null;
        }


        var primeTcrToCheck = new PrimeTcrItemForList
        {
            PrimeTcrStatusId = primeTcr.PrimeTcrStatusId,
            VerificationStatusId = primeTcr.VerificationStatusId,
            HaveDocument = primeTcr.HasDocument,
            Invoiced = primeTcr.Invoiced,
            HalfCommissionPaid = primeTcr.HalfCommissionPaid,
            IsHalfCommission = primeTcr.TcrConfigs.IsHalfCommission,
            CompanyId = primeTcr.CompanyId,
            BranchId = primeTcr.BranchId,
            AgentsIdsArray = primeTcr.AgentsIdsArray,
            ManagersIdsArray = primeTcr.ManagersIdsArray,
            DueBalance = primeTcr.DueBalance,
            IsCompanyCommissionCollected = primeTcr.IsCompanyCommissionCollected,
            ConfirmedHalfContractingDate = primeTcr.ConfirmedHalfContractingDate,
            ConfirmedReservingDate = primeTcr.ConfirmedReservingDate
        };



        allowedActionChecker.PopulateAllowedActions(new[] { primeTcrToCheck });
        primeTcr.AllowedActions = primeTcrToCheck.AllowedActions;


        if (loggedInUserInfo.Privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.Accounting) == false)
        {

            primeTcr.SystemEvents = primeTcr.SystemEvents.Where(z => z.EventProcessId < (int)EventProcesses.RevenueCollected && z.EventProcessId < (int)EventProcesses.SetInvoiced).ToList();
            primeTcr.TcrConfigs = new TcrConfigsItem();

            var privileges = (await lookUpsService.GetPrivilegesAsync())
                .Where(z => z.ParentItemId == (int)PrivilegeCategories.Accounting).Select(z => z.ItemId)
                .ToArray();
            primeTcr.AllowedActions = primeTcr.AllowedActions.Where(z => privileges.Contains((int)z) == false).ToArray();
        }



        return primeTcr;
    }

    public async Task<bool> UpdatePrimeTcrConfigsAsync(PrimeTcrFullItem model)
    {
        var privilege = CheckPrivilege(SystemPrivileges.PrimeTcrsCanUpdateConfigsAndCommissions);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }
        var primeTcrEntities = await repo.GetPrimeTcrsEntitiesAsync([model.PrimeTcrId]);

        if (primeTcrEntities.Length == 0)
        {
            LastErrors.Add(Errors.ItemDoesNotExistOrYouDoNotHavePrivilegeToAccess);
            return false;
        }
        var primeTcr = primeTcrEntities[0];
        var primeTcrToCheck = new PrimeTcrItemForList
        {
            PrimeTcrStatusId = primeTcr.PrimeTcrStatusId,
            VerificationStatusId = primeTcr.VerificationStatusId,
            HaveDocument = primeTcr.HaveDocument,
            Invoiced = primeTcr.Invoiced,
            HalfCommissionPaid = primeTcr.HalfCommissionPaid,
            IsHalfCommission = primeTcr.IsHalfCommission,
            CompanyId = primeTcr.CompanyId,
            BranchId = primeTcr.BranchId,
            AgentsIdsArray = primeTcr.AgentsIdsArray,
            ManagersIdsArray = primeTcr.ManagersIdsArray,
            DueBalance = primeTcr.DueBalance,
            IsCompanyCommissionCollected = primeTcr.IsCompanyCommissionCollected,
            ConfirmedHalfContractingDate = primeTcr.ConfirmedHalfContractingDate,
            ConfirmedReservingDate = primeTcr.ConfirmedReservingDate
        };
        var privilegeItem = allowedActionChecker.CanUpdateConfigsAndCommissions(primeTcrToCheck);
        if (privilegeItem == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }
        primeTcr.CompanyCommissionPercentage = model.TcrConfigs.CompanyCommissionPercentage * 0.01M;
        primeTcr.CompanyCommissionValue = primeTcr.SalesVolume * primeTcr.CompanyCommissionPercentage;
        primeTcr.ForceAchievementPercentage = model.TcrConfigs.ForceAchievementPercentage * 0.01M;
        primeTcr.ForceCommissionPercentage = model.TcrConfigs.ForceCommissionPercentage * 0.01M;
        primeTcr.ForcedAgentIncentiveValue = model.TcrConfigs.ForcedAgentIncentiveValue;
        primeTcr.ForcedScaledCommissionPercentage = model.TcrConfigs.ForcedScaledCommissionPercentage * 0.01M;
        primeTcr.ForceFlatRateCommission = model.TcrConfigs.ForceFlatRateCommission;
        primeTcr.ForceHalfDeal = model.TcrConfigs.ForceHalfDeal;
        primeTcr.FreezeCommissionAgentsIds = string.Join(",", model.TcrConfigs.FreezeCommissionAgentsIds);
        primeTcr.FreezeCommissionManagersIds = string.Join(",", model.TcrConfigs.FreezeCommissionManagersIds);
        primeTcr.IgnoreDebitedCommission = model.TcrConfigs.IgnoreDebitedCommission;
        primeTcr.IgnoreDebitedIncentive = model.TcrConfigs.IgnoreDebitedIncentive;
        primeTcr.IsHalfCommission = model.TcrConfigs.IsHalfCommission;
        primeTcr.IsRegular = model.TcrConfigs.IsRegular;
        primeTcr.RestrictTargetCommission = model.TcrConfigs.RestrictTargetCommission;
        primeTcr.SkipIncentive = model.TcrConfigs.SkipIncentive;
        primeTcr.SkipHalfCommissionRules = model.TcrConfigs.SkipHalfCommissionRules;
        primeTcr.TaxPercentage = model.TcrConfigs.TaxPercentage * 0.01M;
        primeTcr.TcrSelection = model.TcrConfigs.TcrSelection;
        primeTcr.ResignedRuleSkippedIds = string.Join(",", model.TcrConfigs.ResignedRuleSkippedIds);
        await repo.UpdateEntitiesAsync(new List<PrimeTcr> { primeTcr });
        await systemEventsService.GeneratePrimeTcrsActionsEventsAsync(new List<PrimeTcr> { primeTcr }, new PrimeTcrEntityActionModel { EventComment = $"Prime Tcr configs and commissions have been update by {loggedInUserInfo.FullName}", EventProcess = (int)EventProcesses.UpdateTcrProjectConfigs });

        return true;
    }

    public async Task<bool> UpdatePrimeTcrAgentTreeAsync(long primeTcrId, int agentId)
    {

        var privilege = CheckPrivilege(SystemPrivileges.PrimeTcrsCanAccessCommissionsTab);

        if (privilege == null)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }
        var primeTcrEntities = await repo.GetPrimeTcrsEntitiesAsync([primeTcrId]);

        if (primeTcrEntities.Length == 0)
        {
            LastErrors.Add(Errors.ItemDoesNotExistOrYouDoNotHavePrivilegeToAccess);
            return false;
        }
        var primeTcr = primeTcrEntities[0];

        var agent = await usersDataService.GetUserTreeAsync(agentId);

        if (agent == null)
        {
            LastErrors.Add(Errors.AgentDoesNotExist);

            return false;
        }


        if (primeTcr.FirstAgentId == agentId)
        {
            primeTcr.FirstAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);
            primeTcr.FirstAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                    .Select(z => z.AgentName)
                    .ToArray());

        }
        if (primeTcr.SecondAgentId == agentId)
        {
            primeTcr.SecondAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);
            primeTcr.SecondAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                    .Select(z => z.AgentName)
                    .ToArray());

        }

        if (primeTcr.ThirdAgentId == agentId)
        {
            primeTcr.ThirdAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);
            primeTcr.ThirdAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                    .Select(z => z.AgentName)
                    .ToArray());

        }

        if (primeTcr.FirstReferralAgentId == agentId)
        {
            primeTcr.FirstReferralAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);
            primeTcr.FirstReferralAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                    .Select(z => z.AgentName)
                    .ToArray());

        }
        if (primeTcr.SecondReferralAgentId == agentId)
        {
            primeTcr.SecondReferralAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);
            primeTcr.SecondReferralAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                    .Select(z => z.AgentName)
                    .ToArray());

        }

        UpdateManagersArray(primeTcr);

        await repo.UpdateEntitiesAsync([primeTcr]);

        return true;
    }

    private void UpdateManagersArray(PrimeTcr primeTcr)
    {

        var managersList = new List<int>();


        if (primeTcr.FirstAgentId > 0 && primeTcr.FirstAgentTreeJsonb != null)
        {
            var obj = UtilityFunctions.DeserializeJsonDocument<SalesUsersList>(primeTcr.FirstAgentTreeJsonb);

            managersList.AddRange(obj.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (primeTcr.SecondAgentId > 0 && primeTcr.SecondAgentTreeJsonb != null)
        {
            var obj = UtilityFunctions.DeserializeJsonDocument<SalesUsersList>(primeTcr.SecondAgentTreeJsonb);

            managersList.AddRange(obj.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }


        if (primeTcr.ThirdAgentId > 0 && primeTcr.ThirdAgentTreeJsonb != null)
        {
            var obj = UtilityFunctions.DeserializeJsonDocument<SalesUsersList>(primeTcr.ThirdAgentTreeJsonb);

            managersList.AddRange(obj.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (primeTcr.FirstReferralAgentId > 0 && primeTcr.FirstReferralAgentTreeJsonb != null)
        {
            var obj = UtilityFunctions.DeserializeJsonDocument<SalesUsersList>(primeTcr.FirstReferralAgentTreeJsonb);

            managersList.AddRange(obj.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (primeTcr.SecondReferralAgentId > 0 && primeTcr.SecondReferralAgentTreeJsonb != null)
        {
            var obj = UtilityFunctions.DeserializeJsonDocument<SalesUsersList>(primeTcr.SecondReferralAgentTreeJsonb);

            managersList.AddRange(obj.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        primeTcr.ManagersIdsArray = managersList.Distinct()
            .ToArray();
    }

    private async Task SettingAgentSearchCriteria(PrimeTcrsFiltersModel filters)
    {
        if (filters.AgentId == 0)
        {
            if (filters.ManagersIds.Any())
            {
                filters.AgentId = filters.ManagersIds.First();
                filters.UserAgentOr = true;
            }
            return;
        }

        var agent = await usersDataService.GetUserTreeAsync(filters.AgentId);

        if (agent == null)
        {
            return;
        }

        if (agent.IsManager == false && agent.CanHaveTeam == false)
        {
            filters.AgentsRecursive = false;

            return;
        }

        if (filters.AgentsRecursive)
        {
            filters.UserAgentOr = true;
            filters.ManagersIds.Add(filters.AgentId);
        }
    }

    private void PopulateSalesPersonsLists(PrimeTcrsFiltersModel filters, UserPrivilegeItem privilege)
    {
        filters.ManagersIds.Clear();
        if (privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            filters.AgentId = loggedInUserInfo.UserId;
            filters.AgentsRecursive = false;

            return;
        }

        if (loggedInUserInfo.TeamsIds.Length > 0)
        {
            filters.ManagersIds.AddRange(loggedInUserInfo.TeamsIds);


            return;
        }

        filters.ManagersIds.Add(loggedInUserInfo.UserId);

    }

    private async Task PopulatesAgentsTrees(CreatePrimeTcrFullModel model)
    {
        var agent = await usersDataService.GetUserTreeAsync(model.FirstAgentId);
        var agentsList = new List<int>();
        var managersList = new List<int>();

        if (agent != null)
        {
            model.FirstAgentInResaleTeam = agent.InResaleTeam;
            model.FirstAgentName = agent.AgentName;
            model.FirstAgentPositionId = agent.PositionId;
            model.FirstAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);

            model.FirstAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                .Select(z => z.AgentName)
                .ToArray());
            agentsList.Add(agent.UserId);
            managersList.AddRange(agent.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (model.SecondAgentId > 0 && (agent = await usersDataService.GetUserTreeAsync(model.SecondAgentId)) != null)
        {
            model.SecondAgentInResaleTeam = agent.InResaleTeam;
            model.SecondAgentName = agent.AgentName;
            model.SecondAgentPositionId = agent.PositionId;
            model.SecondAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);

            model.SecondAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                .Select(z => z.AgentName)
                .ToArray());
            agentsList.Add(agent.UserId);
            managersList.AddRange(agent.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (model.ThirdAgentId > 0 && (agent = await usersDataService.GetUserTreeAsync(model.ThirdAgentId)) != null)
        {
            model.ThirdAgentInResaleTeam = agent.InResaleTeam;
            model.ThirdAgentName = agent.AgentName;
            model.ThirdAgentPositionId = agent.PositionId;
            model.ThirdAgentTreeJsonb = UtilityFunctions.SerializeToJsonDocument(agent.SalesUsers);

            model.ThirdAgentTreeNames = string.Join(", ", agent.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
                .Select(z => z.AgentName)
                .ToArray());
            agentsList.Add(agent.UserId);
            managersList.AddRange(agent.SalesUsers.SalesUsers.Select(z => z.UserId).Distinct().ToArray());

        }

        if (model.ExtraManagerId > 0 && (agent = await usersDataService.GetUserTreeAsync(model.ExtraManagerId)) != null)
        {
            model.ExtraManagerName = agent.AgentName;
        }

        model.AgentsIdsArray = agentsList.Distinct()
            .ToArray();

        model.ManagersIdsArray = managersList.Distinct()
            .ToArray();
    }

    private async Task<CreatePrimeTcrFullModel?> PopulateValidBasicData(CreatePrimeTcrModel sourceModel)
    {
        var projects = await developersService.GetProjectsEntitiesAsync(new[] { sourceModel.ProjectId });

        if (projects.Length == 0)
        {
            LastErrors.Add(Errors.ProjectDoesNotExists);

            return null;
        }
        var project = projects[0];
        var leadTicket = await leadTicketsService.GetLeadTicketForPrimeTcrAsync(sourceModel.LeadTicketId);

        if (leadTicket == null)
        {
            return null;
        }

        var unitNumberExist = await repo.CheckUnitNumberExistAsync(sourceModel.UnitNumber, sourceModel.ProjectId,
            new int[(int)PrimeTcrStatuses.CanceledByDeveloper]);

        if (unitNumberExist > 0)
        {
            LastErrors.Add(Errors.UnitNumberAlreadyExist);

            return null;
        }

        var model = new CreatePrimeTcrFullModel
        {
            BranchId = leadTicket.BranchId,
            BuildUpArea = sourceModel.BuildUpArea,
            ClientId = leadTicket.ClientId,
            ClosingChannelExtraId = leadTicket.KnowSubSourceId,
            ClosingChannelId = leadTicket.KnowSourceId,
            CompanyCommissionPercentage = project.CompanyRevenuePercentage,
            CompanyId = leadTicket.CompanyId,
            TeleSalesAgentId = leadTicket.TeleSalesAgentId,
            TeleSalesAgentName = leadTicket.TeleSalesAgentName,
            ContractExpectedDate =
                sourceModel.ContractExpectedDate == null
                    ? null
                    : UtilityFunctions.ConvertToUtc(sourceModel.ContractExpectedDate),
            DeveloperId = sourceModel.DeveloperId,
            DeveloperName = project.Developer.DeveloperName,
            DocumentDate = sourceModel.DocumentDate == null ? sourceModel.DocumentDate : UtilityFunctions.GetUtcDateTime(sourceModel.DocumentDate.Value),
            DocumentTypeId = sourceModel.DocumentTypeId,
            ExtraManagerId = sourceModel.ExtraManagerId,
            FirstAgentId = leadTicket.AgentId,
            ClientName = leadTicket.ClientName,
            FirstAgentSharePercentage = sourceModel.FirstAgentSharePercentage * 0.01m,
            ForceAchievementPercentage = project.ForceAchievementPercentage,
            ForceCommissionPercentage = project.ForceCommissionPercentage,
            ForceFlatRateCommission = project.ForceFlatRateCommission,
            ForceHalfDeal = project.ForceHalfDeal,
            HasDocumentId = sourceModel.HasDocumentId,
            HaveDocument = sourceModel.HasDocumentId == 1,
            IsCorporate = leadTicket.IsCorporate,
            IsHalfCommission = project.Developer.HalfCommission,
            IsRecContracted = sourceModel.IsRecContracted,
            IsRegular = project.IsRegular,
            TcrSelection = project.Selection,
            LandArea = sourceModel.LandArea,
            LastMarketingChannelExtraId = leadTicket.KnowSubSourceId,
            LastMarketingChannelId = leadTicket.KnowSourceId,
            LeadTicketId = sourceModel.LeadTicketId,
            OutsideBrokerId = sourceModel.OutsideBrokerId,
            Phase = sourceModel.Phase,
            ProjectId = sourceModel.ProjectId,
            ProjectName = project.ProjectName,
            PropertyTypeId = sourceModel.PropertyTypeId,
            Remarks = sourceModel.Remarks,
            SalesVolume = sourceModel.SalesVolume,
            SecondAgentId = sourceModel.SecondAgentId,
            SecondAgentSharePercentage = sourceModel.SecondAgentSharePercentage * 0.01m,
            ThirdAgentId = sourceModel.ThirdAgentId,
            ThirdAgentSharePercentage = sourceModel.ThirdAgentSharePercentage * 0.01m,
            UnitNumber = sourceModel.UnitNumber,
            UsageId = leadTicket.UsageId,
            CreatedById = loggedInUserInfo.UserId
        };

        model.CompanyCommissionValue = model.SalesVolume * model.CompanyCommissionPercentage;
        model.CreationDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
        model.CreationDateTime = DateTime.UtcNow;
        model.CreatedBy = loggedInUserInfo.FullName;
        model.DueBalance = model.CompanyCommissionValue;

        if (model.OutsideBrokerId > 0)
        {
            var item = (await lookUpsService.GetOutsideBrokersAsync()).FirstOrDefault(z =>
                z.ItemId == model.OutsideBrokerId);

            if (item != null)
            {
                model.OutsideBrokerName = item.ItemName;
                decimal.TryParse(item.ExtraId, out var percentage);
                model.OutsideBrokerCommissionPercentage = percentage;
                model.OutsideBrokerCommissionValue = percentage * model.SalesVolume;
            }
        }

        return model;
    }

    private bool CanCreatePrimeTcr(long leadTicketId)
    {
        var privilege = CheckPrivilege(SystemPrivileges.LeadTicketsConvertToPrimeTcr);

        if (privilege == null || privilege.PrivilegeScope == PrivilegeScopes.Denied)
        {
            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return false;
        }

        return true;
    }

    protected override void PopulateInitialData()
    {
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }
}