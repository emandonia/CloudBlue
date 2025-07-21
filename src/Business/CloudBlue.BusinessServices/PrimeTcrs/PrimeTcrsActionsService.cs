using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using System.Linq.Dynamic.Core;

namespace CloudBlue.BusinessServices.PrimeTcrs;

public class PrimeTcrsActionsService(
    ILookUpsService lookUpsService, CloudBlueSettings cloudBlueSettings,

    IDevelopersService developersService,
    IPrimeTcrsRepository repo,
    LoggedInUserInfo loggedInUserInfo,
    ISystemEventsService systemEventsService,
    INotificationsService notificationsService, ILeadTicketsService leadTicketsService,
    IPrimeTcrAllowedActionChecker allowedActionChecker) : BaseService, IPrimeTcrsActionsService
{
    public async Task<List<EntityActionResult>> ApplyActionAsync(PrimeTcrEntityActionModel model)
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



        if (model.Action == SystemPrivileges.PrimeTcrsCanReloadCommissions)
        {
            await ReloadCommissionsAsync(model, retObj, privilege);
        }
        else if (
           model.Action == SystemPrivileges.PrimeTcrsCanSetConflict ||
           model.Action == SystemPrivileges.PrimeTcrsCanSetReopen ||
           model.Action == SystemPrivileges.PrimeTcrsSetResolved ||
           model.Action == SystemPrivileges.PrimeTcrsCanDelete ||
           model.Action == SystemPrivileges.PrimeTcrsCanSetCanceledByDeveloper ||
           model.Action == SystemPrivileges.PrimeTcrsCanSetReviewing)
        {
            await ApplyStatusChangeAsync(model, retObj, privilege);
        }
        else if (
            model.Action == SystemPrivileges.PrimeTcrsUpdateSalesVolume ||
            model.Action == SystemPrivileges.PrimeTcrsUpdateUnitType ||
            model.Action == SystemPrivileges.PrimeTcrsUpdateUnitNumber ||
            model.Action == SystemPrivileges.PrimeTcrsAddExtraManager ||
            model.Action == SystemPrivileges.PrimeTcrsAddDocumentDate ||
            model.Action == SystemPrivileges.PrimeTcrsChangeMarketingChannel ||
            model.Action == SystemPrivileges.PrimeTcrsCanSetInvoiced)
        {
            await ApplyValuesChangeAsync(model, retObj, privilege);
        }
        else if (
    model.Action == SystemPrivileges.PrimeTcrsSetContracted ||
    model.Action == SystemPrivileges.PrimeTcrsBulkSetContracted ||
    model.Action == SystemPrivileges.PrimeTcrsCanSetPostpone ||

    model.Action == SystemPrivileges.PrimeTcrsCanUpdateCreationDate ||

    model.Action == SystemPrivileges.PrimeTcrsSetConfirmedContracted ||
    model.Action == SystemPrivileges.PrimeTcrsCanUpdateConfirmationDate ||

    model.Action == SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted ||
    model.Action == SystemPrivileges.PrimeTcrsUpdateHalfConfirmedContracted ||

    model.Action == SystemPrivileges.PrimeTcrsSetConfirmedReserved ||
    model.Action == SystemPrivileges.PrimeTcrsUpdateConfirmedReservedDate ||

    model.Action == SystemPrivileges.PrimeTcrsCanSetHalfCommissionCollected)
        {
            await ApplyDatesActionsAsync(model, retObj, privilege);
        }
        else if (model.Action == SystemPrivileges.PrimeTcrsAddAttachments)
        {
            await AddPrimeTcrsAttachmentAsync(model, retObj, privilege);
        }


        return retObj;
    }

    private async Task AddPrimeTcrsAttachmentAsync(PrimeTcrEntityActionModel model, List<EntityActionResult> retObj, UserPrivilegeItem privilegeItem)
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

        var primeTcrs = await repo.GetPrimeTcrsEntitiesAsync(model.ItemsIds);

        foreach (var result in retObj)
        {
            if (model.FileStream == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var primeTcr = primeTcrs.FirstOrDefault(z => z.Id == result.ItemId);

            if (primeTcr == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new PrimeTcrItemForList
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

            var privilege = allowedActionChecker.CanAddAttachments(itemToCheck);

            if (privilege == null)
            {
                result.Message = errorMessage;


                continue;
            }

            var attachment = new PrimeTcrAttachment()
            {
                PrimeTcrId = primeTcr.Id,
                AttachmentDate = DateTime.UtcNow,
                OriginalFileName = model.FileName ?? "",
                TcrAttachmentDescription = model.Comment ?? "",
                UploadedBy = loggedInUserInfo.FullName,
                UploadedById = loggedInUserInfo.UserId,
            };

            await repo.SaveAttachmentAsync(attachment);
            string attachmentFolder = Path.Combine(cloudBlueSettings.AttachmentsPath, primeTcr.Id.ToString());

            if (Directory.Exists(attachmentFolder) == false)
            {
                Directory.CreateDirectory(attachmentFolder);
            }

            var extension = Path.GetExtension(model.FileName);

            var targetPath = $"{attachmentFolder}\\{attachment.Id}{extension}";
            FileStream outputStream = new(targetPath, FileMode.Create);
            await model.FileStream.CopyToAsync(outputStream);
            await outputStream.DisposeAsync();
            // Ensure stream is disposed properly


            result.ActionResult = true;
            result.Message = "File uploaded successfully";
        }

        if (model.FileStream != null)
        {
            await model.FileStream.DisposeAsync();

        }

    }


    private async Task ReloadCommissionsAsync(PrimeTcrEntityActionModel model, List<EntityActionResult> retObj, UserPrivilegeItem privilegeItem)
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

        var primeTcrs = await repo.GetPrimeTcrsEntitiesAsync(model.ItemsIds);
        var affectedPrimeTcrs = new List<PrimeTcr>();

        var projects = await developersService.GetProjectsEntitiesAsync(primeTcrs.Select(z => z.ProjectId)
            .Distinct()
            .ToArray());

        var outsideBrokers = (await lookUpsService.GetOutsideBrokersAsync()).ToArray();
        foreach (var result in retObj)
        {
            var primeTcr = primeTcrs.FirstOrDefault(z => z.Id == result.ItemId);

            if (primeTcr == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new PrimeTcrItemForList
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

            var privilege = allowedActionChecker.CanPrimeTcrsCanReloadCommissions(itemToCheck);

            if (privilege == null)
            {
                result.Message = errorMessage;

                continue;
            }
            var project = projects.FirstOrDefault(z => z.Id == primeTcr.ProjectId);
            if (project == null)
            {
                result.Message = "Project Does not Exist";

                continue;
            }
            if (primeTcr.OutsideBrokerId > 0)
            {
                var item = outsideBrokers.FirstOrDefault(z =>
                    z.ItemId == primeTcr.OutsideBrokerId);

                if (item == null)
                {
                    result.Message = "Outside Broker Does not Exist";

                    continue;
                }

                primeTcr.OutsideBrokerName = item.ItemName;
                decimal.TryParse(item.ExtraId, out var percentage);
                primeTcr.OutsideBrokerCommissionPercentage = percentage;
                primeTcr.OutsideBrokerCommissionValue = percentage * primeTcr.SalesVolume;

            }
            primeTcr.CompanyCommissionPercentage = project.CompanyRevenuePercentage;
            primeTcr.CompanyCommissionValue = project.CompanyRevenuePercentage * primeTcr.SalesVolume;


            result.ActionResult = true;
            result.Message = "Action applied successfully";







            affectedPrimeTcrs.Add(primeTcr);
        }



        if (affectedPrimeTcrs.Count == 0)
        {
            return;
        }


        await repo.UpdateEntitiesAsync(affectedPrimeTcrs);


        model.EventProcess = (int)EventProcesses.UpdateReloadCommissions;
        model.EventComment = $"Prime Tcr Revenue Percentage was reloaded by {loggedInUserInfo.FullName}";
        await systemEventsService.GeneratePrimeTcrsActionsEventsAsync(affectedPrimeTcrs, model);

    }

    private async Task ApplyDatesActionsAsync(PrimeTcrEntityActionModel model, List<EntityActionResult> bulkActionResults, UserPrivilegeItem privilegeItem)
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

        var primeTcrs = await repo.GetPrimeTcrsEntitiesAsync(model.ItemsIds);
        var affectedPrimeTcrs = new List<PrimeTcr>();
        var statusId = 0;
        var eventComment = "";
        int eventProcess = 0;



        foreach (var result in bulkActionResults)
        {
            var primeTcr = primeTcrs.FirstOrDefault(z => z.Id == result.ItemId);

            if (primeTcr == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new PrimeTcrItemForList
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

            SystemPrivileges? privilege;

            if (model.Action == SystemPrivileges.PrimeTcrsSetContracted ||
            model.Action == SystemPrivileges.PrimeTcrsBulkSetContracted)
            {
                privilege = allowedActionChecker.CanSetContracted(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.CloseTcr;
                eventComment = $"Prime TCR Status was set to Contracted  by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.Contracted;
                primeTcr.RecCloseDate = DateTime.UtcNow;
                primeTcr.RecCloseDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.RecCloseDate);


            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanUpdateCreationDate)
            {
                privilege = allowedActionChecker.CanUpdateCreationDate(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR creation date was updated by: {loggedInUserInfo.FullName}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.CreationDateTime = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.RecReserveDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.RecReserveDate);

            }

            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetPostpone)
            {
                privilege = allowedActionChecker.CanSetPostpone(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.PostponeTcr;
                eventComment = $"Prime TCR was set to Postpone to {model.GenericDate.Value:dd MMM, yyyy} by {loggedInUserInfo.FullName}. Reason: {model.Comment}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.LastPostponeDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.LastPostponeDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastPostponeDate);
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.Postponed;
                primeTcr.SalesAccountingFeedBackId = (int)PrimeTcrStatuses.Postponed;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);


            }


            else if (model.Action == SystemPrivileges.PrimeTcrsSetConfirmedReserved)
            {
                privilege = allowedActionChecker.CanSetConfirmedReserved(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.ConfirmReserveTcr;
                eventComment = $"Prime TCR  reservation was  confirmed by {loggedInUserInfo.FullName} upon developer reviewing ";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ConfirmedReservingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedReservingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedReservingDate);
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.ConfirmedReserved;
                primeTcr.SalesAccountingFeedBackId = (int)PrimeTcrStatuses.ConfirmedReserved;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);
            }

            else if (model.Action == SystemPrivileges.PrimeTcrsUpdateConfirmedReservedDate)
            {
                privilege = allowedActionChecker.CanUpdateConfirmedReservedDate(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime Tcr reservation confirmation date was updated by:  {loggedInUserInfo.FullName}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";

                primeTcr.ConfirmedReservingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedReservingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedReservingDate);
            }


            else if (model.Action == SystemPrivileges.PrimeTcrsSetConfirmedContracted)
            {
                privilege = allowedActionChecker.CanSetConfirmedContracted(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.ConfirmCloseTcr;
                eventComment = $"Prime TCR  contract was  confirmed by {loggedInUserInfo.FullName} upon developer reviewing ";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ConfirmedContractingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedContractingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedContractingDate);
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.ConfirmedContracted;
                primeTcr.SalesAccountingFeedBackId = (int)PrimeTcrStatuses.ConfirmedContracted;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);



            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanUpdateConfirmationDate)
            {
                privilege = allowedActionChecker.CanUpdateConfirmationDate(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime Tcr contract confirmation date was updated by:  {loggedInUserInfo.FullName}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ConfirmedContractingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedContractingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedContractingDate);

            }




            else if (model.Action == SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted)
            {
                privilege = allowedActionChecker.CanSetHalfConfirmedContracted(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.HalfConfirmCloseTcr;
                eventComment = $"Prime TCR  half contract was  confirmed by {loggedInUserInfo.FullName} upon developer reviewing ";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ConfirmedHalfContractingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedHalfContractingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedHalfContractingDate);
                primeTcr.IsHalfContracted = true;
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.HalfConfirmedContracted;
                primeTcr.SalesAccountingFeedBackId = (int)PrimeTcrStatuses.HalfConfirmedContracted;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);
            }

            else if (model.Action == SystemPrivileges.PrimeTcrsUpdateHalfConfirmedContracted)
            {
                privilege = allowedActionChecker.CanUpdateHalfConfirmedContractedDate(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime Tcr half contract confirmation date was updated by:  {loggedInUserInfo.FullName}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ConfirmedHalfContractingDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.ConfirmedHalfContractingDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.ConfirmedHalfContractingDate);

            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetHalfCommissionCollected)
            {
                privilege = allowedActionChecker.CanSetHalfCommissionPaid(itemToCheck);

                if (privilege == null || model.GenericDate == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.SetHalfCommissionPaid;
                eventComment = $"Prime TCR was set to Half Commission Paid by {loggedInUserInfo.FullName}";

                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.HalfCommissionPaid = true;

            }






            affectedPrimeTcrs.Add(primeTcr);

        }



        if (affectedPrimeTcrs.Count == 0)
        {
            return;
        }


        await repo.UpdateEntitiesAsync(affectedPrimeTcrs);

        if (statusId > 0)
        {
            await leadTicketsService.UpdateLeadTicketTcrStatusAsync(affectedPrimeTcrs.Select(z => z.LeadTicketId)
                .ToList(), statusId, EntityTypes.PrimeTcr);
        }
        model.EventProcess = eventProcess;
        model.EventComment = eventComment;
        await systemEventsService.GeneratePrimeTcrsActionsEventsAsync(affectedPrimeTcrs, model);
    }










    private async Task ApplyValuesChangeAsync(PrimeTcrEntityActionModel model, List<EntityActionResult> bulkActionResults, UserPrivilegeItem privilegeItem)
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

        var primeTcrs = await repo.GetPrimeTcrsEntitiesAsync(model.ItemsIds);
        var affectedPrimeTcrs = new List<PrimeTcr>();
        var eventComment = "";
        int eventProcess = 0;


        foreach (var result in bulkActionResults)
        {
            var primeTcr = primeTcrs.FirstOrDefault(z => z.Id == result.ItemId);

            if (primeTcr == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new PrimeTcrItemForList
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

            SystemPrivileges? privilege;

            if (model.Action == SystemPrivileges.PrimeTcrsUpdateSalesVolume)
            {
                privilege = allowedActionChecker.CanUpdateSalesVolume(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Sales Volume was updated by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.SalesVolume = model.NumericValue;

            }
            else if (model.Action == SystemPrivileges.PrimeTcrsUpdateUnitNumber)
            {
                privilege = allowedActionChecker.CanUpdateUnitNumber(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                var unitNumberExist = await repo.CheckUnitNumberExistAsync(model.StringValue, primeTcr.ProjectId,
           new int[(int)PrimeTcrStatuses.CanceledByDeveloper]);

                if (unitNumberExist > 0 && unitNumberExist != primeTcr.Id)
                {
                    result.Message = UtilityFunctions.BreakDownWord(Errors.UnitNumberAlreadyExist.ToString());
                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Unit Number was updated by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.UnitNumber = model.StringValue ?? primeTcr.UnitNumber;

            }

            else if (model.Action == SystemPrivileges.PrimeTcrsAddDocumentDate)
            {
                privilege = allowedActionChecker.CanAddDocumentDate(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Document Date was updated by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.DocumentDate = UtilityFunctions.GetUtcDateTime(model.GenericDate.Value);
                primeTcr.DocumentTypeId = model.SelectedItemId;
                primeTcr.HaveDocument = true;

            }


            else if (model.Action == SystemPrivileges.PrimeTcrsUpdateUnitType)
            {
                privilege = allowedActionChecker.CanUpdateUnitType(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Sales Volume was updated by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.PropertyTypeId = model.SelectedItemId;

            }
            else if (model.Action == SystemPrivileges.PrimeTcrsChangeMarketingChannel)
            {
                privilege = allowedActionChecker.CanChangeMarketingChannel(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Marketing Channel was updated by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ClosingChannelId = model.KnowSourceId;
                primeTcr.ClosingChannelExtraId = model.KnowSubSourceId;

            }
            else if (model.Action == SystemPrivileges.PrimeTcrsAddExtraManager)
            {
                privilege = allowedActionChecker.CanAddExtraManager(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.UpdateTcr;
                eventComment = $"Prime TCR Extra Manager was added by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.ExtraManagerId = model.SelectedItemId;
                primeTcr.ExtraManagerName = model.StringValue;
            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetInvoiced)
            {
                privilege = allowedActionChecker.CanSetInvoiced(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.SetInvoiced;
                eventComment = $"Prime TCR  was set to invoiced  by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                primeTcr.Invoiced = true;

            }
            affectedPrimeTcrs.Add(primeTcr);

        }



        if (affectedPrimeTcrs.Count == 0)
        {
            return;
        }


        await repo.UpdateEntitiesAsync(affectedPrimeTcrs);


        model.EventProcess = eventProcess;
        model.EventComment = eventComment;
        await systemEventsService.GeneratePrimeTcrsActionsEventsAsync(affectedPrimeTcrs, model);

    }

    private async Task ApplyStatusChangeAsync(PrimeTcrEntityActionModel model,
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

        var primeTcrs = await repo.GetPrimeTcrsEntitiesAsync(model.ItemsIds);
        var affectedPrimeTcrs = new List<PrimeTcr>();
        var statusId = 0;
        var eventComment = "";
        int eventProcess = 0;



        foreach (var result in bulkActionResults)
        {
            var primeTcr = primeTcrs.FirstOrDefault(z => z.Id == result.ItemId);

            if (primeTcr == null)
            {
                result.Message = UtilityFunctions.BreakDownWord(Errors.ItemNotFound.ToString());

                continue;
            }

            var itemToCheck = new PrimeTcrItemForList
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

            SystemPrivileges? privilege;

            if (model.Action == SystemPrivileges.PrimeTcrsCanSetReviewing)
            {
                privilege = allowedActionChecker.CanSetReviewing(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;
                    //continue;
                }
                eventProcess = (int)EventProcesses.ReviewingByDeveloper;
                eventComment = $"Prime TCR Feedback was set to sent to developer for reviewing by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.ReviewingByDeveloper;
                primeTcr.LastDeveloperReviewingDate = DateTime.UtcNow;
                primeTcr.LastDeveloperReviewingDateNumeric = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);

            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetConflict)
            {
                privilege = allowedActionChecker.CanSetConflict(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.ConflictTcr;
                eventComment = $"Prime TCR was set to Conflict by {loggedInUserInfo.FullName}. Reason: {model.Comment}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.Conflict;
                primeTcr.LastConflictDate = DateTime.UtcNow;

                primeTcr.LastConflictDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastConflictDate);
                primeTcr.LastDeveloperFeedBack = eventComment;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastDeveloperFeedBackDate);
                primeTcr.SalesAccountingFeedBackId = statusId;




            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetReopen)
            {
                privilege = allowedActionChecker.CanSetReopen(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.ReopenTcr;
                result.ActionResult = true;
                result.Message = "Action applied successfully";

                statusId = primeTcr.PrimeTcrStatusId = (primeTcr.PrimeTcrStatusId == (int)PrimeTcrStatuses.ReviewingByDeveloper) ? (int)PrimeTcrStatuses.ReopenDev : (int)PrimeTcrStatuses.ReopenSales;
                primeTcr.IsReOpen = true;

                primeTcr.LastReopenDate = DateTime.UtcNow;
                primeTcr.LastReOpenReason = model.Comment;
                primeTcr.LastReopenDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastReopenDate);



                eventComment = $"Prime TCR was set to Re_Open by {loggedInUserInfo.FullName}. Reason: {model.Comment}";


                primeTcr.LastDeveloperFeedBack = eventComment;
                primeTcr.LastFeedBack = model.Comment;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastDeveloperFeedBackDate);
                primeTcr.SalesAccountingFeedBackId = statusId;



            }
            else if (model.Action == SystemPrivileges.PrimeTcrsSetResolved)
            {
                privilege = allowedActionChecker.CanSetResolved(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.ResolvedByRec;
                eventComment = $"Prime TCR Status was set to Resolved  by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.Resolved;
                primeTcr.IsReOpen = false;
                primeTcr.LastResolveDate = DateTime.UtcNow;
                primeTcr.LastResolveDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastResolveDate);
                primeTcr.IsResolved = true;


            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanDelete)
            {
                privilege = allowedActionChecker.CanDelete(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.DeleteTcr;
                eventComment = $"Prime TCR Status was set to deleted  by: {loggedInUserInfo.FullName}";
                result.ActionResult = true;
                result.Message = "Action applied successfully";
                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.Deleted;
                primeTcr.IsDeleted = true;


            }
            else if (model.Action == SystemPrivileges.PrimeTcrsCanSetCanceledByDeveloper)
            {
                privilege = allowedActionChecker.CanSetCanceledByDeveloper(itemToCheck);

                if (privilege == null)
                {
                    result.Message = errorMessage;

                    continue;
                }
                eventProcess = (int)EventProcesses.CanceledByDeveloper;
                result.ActionResult = true;
                result.Message = "Action applied successfully";

                statusId = primeTcr.PrimeTcrStatusId = (int)PrimeTcrStatuses.CanceledByDeveloper;




                eventComment = $"Prime TCR was set to Canceled by Developer by {loggedInUserInfo.FullName}. Reason: {model.Comment}";

                primeTcr.LastDeveloperFeedBack = eventComment;
                primeTcr.LastDeveloperFeedBackDate = DateTime.UtcNow;
                primeTcr.LastDeveloperFeedBackDateNumeric = UtilityFunctions.GetIntegerFromDate(primeTcr.LastDeveloperFeedBackDate);
                primeTcr.SalesAccountingFeedBackId = statusId;



            }


            affectedPrimeTcrs.Add(primeTcr);

        }



        if (affectedPrimeTcrs.Count == 0)
        {
            return;
        }


        await repo.UpdateEntitiesAsync(affectedPrimeTcrs);

        if (statusId > 0)
        {
            await leadTicketsService.UpdateLeadTicketTcrStatusAsync(affectedPrimeTcrs.Select(z => z.LeadTicketId)
                .ToList(), statusId, EntityTypes.PrimeTcr);
        }
        model.EventProcess = eventProcess;
        model.EventComment = eventComment;
        await systemEventsService.GeneratePrimeTcrsActionsEventsAsync(affectedPrimeTcrs, model);
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