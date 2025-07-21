using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.Crm;

public class LeadTicketsActionsService(
    ILookUpsService lookUpsService,
    ILeadTicketsRepository repo,
    LoggedInUserInfo loggedInUserInfo,
    ISystemEventsService systemEventsService,
    INotificationsService notificationsService,
    ILeadTicketAllowedActionChecker allowedActionChecker) : BaseService, ILeadTicketsActionsService
{
    public async Task<List<EntityActionResult>> ApplyActionAsync(LeadTicketActionModel model)
    {
        var privilege = CheckPrivilege(model.Action);
        var retObj = new List<EntityActionResult>();

        model.ItemsIds.ForEach(z =>
            retObj.Add(new EntityActionResult { ActionResult = false, ItemId = z, Message = string.Empty }));

        if (privilege == null)
        {
            retObj.ForEach(z =>
                z.Message = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString()));

            LastErrors.Add(Errors.YouDoNotHavePrivilegeToDoThisAction);

            return retObj;
        }

        model.CurrentUserName = loggedInUserInfo.FullName;

        if (model.Action == SystemPrivileges.LeadTicketsArchive || model.Action == SystemPrivileges.LeadTicketsUnArchive)
        {
            await ApplyLeadTicketsArchivingAsync(model, retObj, privilege);
        }
        else if (model.Action == SystemPrivileges.LeadTicketsAssignToAgent ||
                model.Action == SystemPrivileges.LeadTicketsReAssignToAgent)
        {
            await ApplyLeadTicketsAssigningToAgentAsync(model, retObj, privilege);
        }
        else if (model.Action == SystemPrivileges.LeadTicketsAssignToBranch ||
                model.Action == SystemPrivileges.LeadTicketsReAssignToBranch ||
                model.Action == SystemPrivileges.LeadTicketsMoveToCompany)
        {
            await ApplyLeadTicketsMovingAsync(model, retObj, privilege);
        }
        else if (model.Action == SystemPrivileges.LeadTicketsSetVoid ||
                model.Action == SystemPrivileges.LeadTicketsReject)
        {
            await ApplyLeadTicketsCancellationAsync(model, retObj, privilege);
        }
        else if (model.Action == SystemPrivileges.LeadTicketsAddEvent || model.Action == SystemPrivileges.LeadTicketsAddFeedback)
        {
            await ApplyLeadTicketsAddingActivityAsync(model, retObj, privilege);
        }

        return retObj;
    }

    private async Task ApplyLeadTicketsAddingActivityAsync(LeadTicketActionModel model,
        List<EntityActionResult> bulkActionResults, PrivilegeItem privilegeItem)
    {
        string errorMessage;

        if (string.IsNullOrEmpty(privilegeItem.PrivilegeMetaData) == false)
        {
            errorMessage = privilegeItem.PrivilegeMetaData;
        }
        else
        {
            errorMessage = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString());
        }

        var leadTickets = await repo.GetLeadTicketEntitiesAsync(model.ItemsIds, true, true);
        var eventContactingTypes = await lookUpsService.GetContactingTypesAsync();
        var contactingType = eventContactingTypes.FirstOrDefault(z => z.Id == model.ContactingTypeId);

        if (contactingType == null)
        {
            return;
        }

        model.SkipTemplate = true;
        model.IsReminder = contactingType.Id == (int)ContactingTypes.Reminder;
        var assigned = (int)LeadTicketStatuses.Assigned;

        foreach (var result in bulkActionResults)
        {
            var leadTicket = leadTickets.FirstOrDefault(z => z.Id == result.ItemId);

            if (leadTicket == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new LeadTicketItemForList
            {
                BranchId = leadTicket.BranchId,
                CompanyId = leadTicket.CompanyId,
                CurrentAgentId = leadTicket.CurrentAgentId,
                LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId
            };

            var privilege = model.Action == SystemPrivileges.LeadTicketsAddEvent ? allowedActionChecker.CanAddEvent(itemToCheck) : allowedActionChecker.CanAddFeedBackEvent(itemToCheck);

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }

            result.ActionResult = true;
            result.Message = "Action applied successfully";

            if (contactingType.IsEssential == false && contactingType.NeedsDate == false)
            {
                continue;
            }

            if (contactingType.IsFeedBack &&
               leadTicket.CurrentAgentId == loggedInUserInfo.UserId)
            {
                leadTicket.LastAgentFeedBackId = model.ContactingTypeId;
                leadTicket.LastAgentFeedBackNote = model.Comment;
                leadTicket.LastAgentFeedBackDate = DateTime.UtcNow;
                leadTicket.LastAgentFeedBackDateNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

                if (leadTicket.LeadTicketStatusId == assigned)
                {
                    leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.InProgress;
                    leadTicket.SetInProgressDate = DateTime.UtcNow;
                    leadTicket.SetInProgressDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
                    leadTicket.SetInProgressDate = DateTime.UtcNow;

                    leadTicket.HoursToGetInProgress = leadTicket.LastAssignedDate == null
                        ? 0
                        : Convert.ToInt32(DateTime.UtcNow.Subtract(leadTicket.LastAssignedDate.Value)
                            .TotalHours);
                }
            }

            if (contactingType.Id == (int)ContactingTypes.CallLater && model.ReminderDate != null)
            {
                leadTicket.CallBackDate = UtilityFunctions.GetUtcDateTime(model.ReminderDate.Value);
                leadTicket.CallBackDateNumeric = long.Parse(model.ReminderDate.Value.ToString("yyyyMMddHHmm"));
            }
        }

        var affectedIds = bulkActionResults.Where(z => z.ActionResult)
            .Select(z => z.ItemId)
            .ToArray();

        if (affectedIds.Length == 0)
        {
            return;
        }

        var currentAgentIds = leadTickets.Select(z => z.CurrentAgentId)
            .Distinct()
            .ToList();

        if (currentAgentIds.Count == 1 && currentAgentIds.First() == loggedInUserInfo.UserId)
        {
            model.CurrentAgentId = loggedInUserInfo.UserId;
        }

        var affectedLeads = leadTickets.Where(l => affectedIds.Contains(l.Id))
            .ToList();

        try
        {

            await repo.UpdateEntitiesAsync(affectedLeads);
            await systemEventsService.GenerateLeadTicketsActionsEventsAsync(affectedLeads, model);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }

    private async Task ApplyLeadTicketsAssigningToAgentAsync(LeadTicketActionModel model,
        List<EntityActionResult> bulkActionResults, UserPrivilegeItem privilegeItem)
    {
        string errorMessage;

        if (string.IsNullOrEmpty(privilegeItem.PrivilegeMetaData) == false)
        {
            errorMessage = privilegeItem.PrivilegeMetaData;
        }
        else
        {
            errorMessage = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString());
        }

        var leadTickets = await repo.GetLeadTicketEntitiesAsync(model.ItemsIds, true, true);

        foreach (var result in bulkActionResults)
        {
            var leadTicket = leadTickets.FirstOrDefault(z => z.Id == result.ItemId);

            if (leadTicket == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new LeadTicketItemForList
            {
                BranchId = leadTicket.BranchId,
                CompanyId = leadTicket.CompanyId,
                CurrentAgentId = leadTicket.CurrentAgentId,
                LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId
            };

            SystemPrivileges? privilege;

            if (model.Action == SystemPrivileges.LeadTicketsAssignToAgent)
            {
                privilege = allowedActionChecker.CanAssignToAgent(itemToCheck);
            }
            else
            {
                privilege = allowedActionChecker.CanReAssignToAgent(itemToCheck);
            }

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }

            result.ActionResult = true;
            result.Message = "Action applied successfully";

            if (privilegeItem.PrivilegeScope == PrivilegeScopes.Global)
            {
                leadTicket.CompanyId = model.CompanyId;
                leadTicket.BranchId = model.BranchId;
            }
            else if (privilegeItem.PrivilegeScope == PrivilegeScopes.Company)
            {
                leadTicket.BranchId = model.BranchId;
            }

            leadTicket.LeadTicketExtension.OldAgentId = leadTicket.CurrentAgentId;
            leadTicket.CurrentAgentId = model.AgentId;

            //leadTicket.LeadTicketExtension.CurrentAgentManagersTree

            if (leadTicket.FirstOwnerId == 0)
            {
                leadTicket.FirstOwnerId = leadTicket.CurrentAgentId;
            }

            leadTicket.LastAssignedDate = DateTime.UtcNow;
            leadTicket.LastAssignedDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Assigned;
            leadTicket.ExtendedDate = null;
            leadTicket.ExtendedDateNumeric = 0;
            leadTicket.ViewedByCurAgent = false;

            if (model.Action == SystemPrivileges.LeadTicketsReAssignToAgent)
            {
                if (leadTicket.SetInProgressDate == null)
                {
                    leadTicket.ReassignedNewOnce++;
                }

                leadTicket.ReassignedOnce = true;
                leadTicket.ReassignCount++;
            }
        }

        var affectedIds = bulkActionResults.Where(z => z.ActionResult)
            .Select(z => z.ItemId)
            .ToArray();

        if (affectedIds.Length == 0)
        {
            return;
        }

        var affectedLeads = leadTickets.Where(l => affectedIds.Contains(l.Id))
            .ToList();

        await PopulateManagersData(affectedLeads);
        await repo.UpdateEntitiesAsync(affectedLeads);

        await systemEventsService.GenerateLeadTicketsActionsEventsAsync(affectedLeads, model);

        var leadsForEmails = await repo.GetLeadTicketForEmailsAsync(affectedIds);

        notificationsService.SendAssignedLeadTicketNotificationAsync(leadsForEmails);
    }

    private async Task PopulateManagersData(List<LeadTicket> affectedLeads)
    {

    }

    private async Task ApplyLeadTicketsMovingAsync(LeadTicketActionModel model, List<EntityActionResult> bulkActionResults,
        UserPrivilegeItem privilegeItem)
    {
        string errorMessage;

        if (string.IsNullOrEmpty(privilegeItem.PrivilegeMetaData) == false)
        {
            errorMessage = privilegeItem.PrivilegeMetaData;
        }
        else
        {
            errorMessage = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString());
        }

        var leadTickets = await repo.GetLeadTicketEntitiesAsync(model.ItemsIds, true, true);

        foreach (var result in bulkActionResults)
        {
            var leadTicket = leadTickets.FirstOrDefault(z => z.Id == result.ItemId);

            if (leadTicket == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new LeadTicketItemForList
            {
                BranchId = leadTicket.BranchId,
                CompanyId = leadTicket.CompanyId,
                CurrentAgentId = leadTicket.CurrentAgentId,
                LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId
            };

            SystemPrivileges? privilege = null;

            if (model.Action == SystemPrivileges.LeadTicketsAssignToBranch)
            {
                privilege = allowedActionChecker.CanAssignToBranch(itemToCheck);
            }
            else if (model.Action == SystemPrivileges.LeadTicketsReAssignToBranch)
            {
                privilege = allowedActionChecker.CanReAssignToBranch(itemToCheck);
            }
            else
            {
                privilege = allowedActionChecker.CanMoveToCompany(itemToCheck);
            }

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }

            result.ActionResult = true;
            result.Message = "Action applied successfully";

            if (privilegeItem.PrivilegeScope == PrivilegeScopes.Global)
            {
                leadTicket.CompanyId = model.CompanyId;
                leadTicket.BranchId = model.BranchId;
            }
            else if (privilegeItem.PrivilegeScope == PrivilegeScopes.Company)
            {
                leadTicket.BranchId = model.BranchId;
            }

            leadTicket.LeadTicketExtension.OldAgentId = leadTicket.CurrentAgentId;
            leadTicket.CurrentAgentId = 0;
            leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Unassigned;

            //if (model.AgentId > 0)

            //{
            //     leadTicket.CurrentAgentId = model.AgentId;

            //    if (leadTicket.FirstOwnerId == 0)
            //    {
            //        leadTicket.FirstOwnerId = leadTicket.CurrentAgentId;
            //    }

            //    leadTicket.LastAssignedDate = DateTime.UtcNow;
            //    leadTicket.LastAssignedDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
            //    leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Assigned;
            //    leadTicket.ExtendedDate = null;
            //    leadTicket.ExtendedDateNumeric = 0;
            //    leadTicket.ViewedByCurAgent = false;

            //    if (leadTicket.SetInProgressDateNum == 0)
            //    {
            //        leadTicket.ReassignedNewOnce++;
            //    }

            //    leadTicket.ReassignedOnce = true;
            //    leadTicket.ReassignCount++;
            //}
        }

        var affectedIds = bulkActionResults.Where(z => z.ActionResult)
            .Select(z => z.ItemId)
            .ToArray();

        if (affectedIds.Length == 0)
        {
            return;
        }

        var affectedLeads = leadTickets.Where(l => affectedIds.Contains(l.Id))
            .ToList();

        await systemEventsService.GenerateLeadTicketsActionsEventsAsync(affectedLeads, model);
    }

    private async Task ApplyLeadTicketsCancellationAsync(LeadTicketActionModel model,
        List<EntityActionResult> bulkActionResults, PrivilegeItem privilegeItem)
    {
        string errorMessage;

        if (string.IsNullOrEmpty(privilegeItem.PrivilegeMetaData) == false)
        {
            errorMessage = privilegeItem.PrivilegeMetaData;
        }
        else
        {
            errorMessage = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString());
        }

        var leadTickets = await repo.GetLeadTicketEntitiesAsync(model.ItemsIds, true, true);
        var events = new List<SystemEventItem>();

        foreach (var result in bulkActionResults)
        {
            var leadTicket = leadTickets.FirstOrDefault(z => z.Id == result.ItemId);

            if (leadTicket == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new LeadTicketItemForList
            {
                BranchId = leadTicket.BranchId,
                CompanyId = leadTicket.CompanyId,
                CurrentAgentId = leadTicket.CurrentAgentId,
                LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId
            };

            SystemPrivileges? privilege = null;

            if (model.Action == SystemPrivileges.LeadTicketsSetVoid)
            {
                privilege = allowedActionChecker.CanSetVoid(itemToCheck);
            }
            else if (model.Action == SystemPrivileges.LeadTicketsReject)
            {
                privilege = allowedActionChecker.CanReject(itemToCheck);
            }

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }

            result.ActionResult = true;
            result.Message = "Action applied successfully";

            if (model.Action == SystemPrivileges.LeadTicketsSetVoid)
            {
                leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Void;
                leadTicket.DateVoided = DateTime.UtcNow;
                leadTicket.IsVoided = true;
                leadTicket.VoidingReason = model.Comment;
                leadTicket.LeadTicketExtension.VoidingLeadTicketId = 0;
                leadTicket.LeadTicketExtension.LastVoidedDate = DateTime.UtcNow;
                leadTicket.LeadTicketExtension.LastVoidedDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd"));
                leadTicket.VoidReasonId = model.ReasonId;
            }
            else
            {
                leadTicket.LeadTicketStatusId = (int)LeadTicketStatuses.Inactive;
                leadTicket.LeadTicketExtension.RejectDate = DateTime.UtcNow;
                leadTicket.LeadTicketExtension.RejectReason = model.Comment;
                leadTicket.RejectReasonId = model.ReasonId;
            }
        }

        var affectedIds = bulkActionResults.Where(z => z.ActionResult)
            .Select(z => z.ItemId)
            .ToArray();

        if (affectedIds.Length == 0)
        {
            return;
        }

        var affectedLeads = leadTickets.Where(l => affectedIds.Contains(l.Id))
            .ToList();

        await repo.UpdateEntitiesAsync(affectedLeads);
        await systemEventsService.GenerateLeadTicketsActionsEventsAsync(affectedLeads, model);
    }

    private async Task ApplyLeadTicketsArchivingAsync(LeadTicketActionModel model,
        List<EntityActionResult> bulkActionResults, PrivilegeItem privilegeItem)
    {
        string errorMessage;

        if (string.IsNullOrEmpty(privilegeItem.PrivilegeMetaData) == false)
        {
            errorMessage = privilegeItem.PrivilegeMetaData;
        }
        else
        {
            errorMessage = UtilityFunctions.BreakDownWord(Errors.YouDoNotHavePrivilegeToDoThisAction.ToString());
        }

        var leadTickets = await repo.GetLeadTicketEntitiesAsync(model.ItemsIds, true, true);

        foreach (var result in bulkActionResults)
        {
            var leadTicket = leadTickets.FirstOrDefault(z => z.Id == result.ItemId);

            if (leadTicket == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new LeadTicketItemForList
            {
                BranchId = leadTicket.BranchId,
                CompanyId = leadTicket.CompanyId,
                CurrentAgentId = leadTicket.CurrentAgentId,
                LeadTicketStatusId = (LeadTicketStatuses)leadTicket.LeadTicketStatusId
            };

            SystemPrivileges? privilege = null;

            if (model.Action == SystemPrivileges.LeadTicketsArchive)
            {
                privilege = allowedActionChecker.CanArchive(itemToCheck);
            }
            else if (model.Action == SystemPrivileges.LeadTicketsUnArchive)
            {
                privilege = allowedActionChecker.CanUnArchive(itemToCheck);
            }

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }

            result.ActionResult = true;
            result.Message = "Action applied successfully";
            leadTicket.IsArchived = model.Action == SystemPrivileges.LeadTicketsArchive;
        }

        var affectedIds = bulkActionResults.Where(z => z.ActionResult)
            .Select(z => z.ItemId)
            .ToArray();

        if (affectedIds.Length == 0)
        {
            return;
        }

        var affectedLeads = leadTickets.Where(l => affectedIds.Contains(l.Id))
            .ToList();

        await repo.UpdateEntitiesAsync(affectedLeads);
        await systemEventsService.GenerateLeadTicketsActionsEventsAsync(affectedLeads, model);
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
}