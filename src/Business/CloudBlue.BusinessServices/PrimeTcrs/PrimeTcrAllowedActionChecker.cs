using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;
using System.Reflection;

namespace CloudBlue.BusinessServices.PrimeTcrs;

public class PrimeTcrAllowedActionChecker(LoggedInUserInfo currentUser) : BaseService, IPrimeTcrAllowedActionChecker
{
    public SystemPrivileges? CanAddExtraManager(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsAddExtraManager;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateUnitType(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsUpdateUnitType;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateSalesVolume(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsUpdateSalesVolume;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateUnitNumber(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsUpdateUnitNumber;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateConfigsAndCommissions(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanUpdateConfigsAndCommissions;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanPrimeTcrsCanReloadCommissions(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanReloadCommissions;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddEvent(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsAddEvent;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddAttachments(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsAddAttachments;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddDocumentDate(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsAddDocumentDate;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId) || item.HaveDocument)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanChangeMarketingChannel(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsChangeMarketingChannel;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.Contracted, (int)PrimeTcrStatuses.Reserved };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanDelete(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanDelete;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper, (int)PrimeTcrStatuses.ReviewingByDeveloper, (int)PrimeTcrStatuses.HalfConfirmedContracted, (int)PrimeTcrStatuses.Postponed };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetHalfConfirmedContracted(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper, (int)PrimeTcrStatuses.HalfConfirmedContracted };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId) || item.ForceHalfDeal == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateHalfConfirmedContractedDate(PrimeTcrItemForList item)
    {

        var retVal = SystemPrivileges.PrimeTcrsSetHalfConfirmedContracted;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.HalfConfirmedContracted };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false || item.ForceHalfDeal == false || item.ConfirmedHalfContractingDate == null)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetConfirmedContracted(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsSetConfirmedContracted;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetConfirmedReserved(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsSetConfirmedReserved;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[] { (int)PrimeTcrStatuses.Reserved };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateConfirmedReservedDate(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsUpdateConfirmedReservedDate;

        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };


        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId) || item.ConfirmedReservingDate == null)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetCanceledByDeveloper(PrimeTcrItemForList item)
    {

        var retVal = SystemPrivileges.PrimeTcrsCanSetCanceledByDeveloper;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ReviewingByDeveloper };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetConflict(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetConflict;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ReviewingByDeveloper };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetContracted(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsSetContracted;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.Reserved };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetHalfCommissionPaid(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetHalfCommissionCollected;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId) || item.HalfCommissionPaid || item.IsHalfCommission == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddPayments(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanAddPayments;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false || item.IsCompanyCommissionCollected || item.DueBalance == 0)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetInvoiced(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetInvoiced;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted };

        if (userPrivilegeItem == null || item.Invoiced || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetPostpone(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetPostpone;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ReviewingByDeveloper };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetReopen(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetReopen;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.ReopenDev, (int)PrimeTcrStatuses.ReopenSales, (int)PrimeTcrStatuses.ConfirmedContracted, (int)PrimeTcrStatuses.ConfirmedReserved, (int)PrimeTcrStatuses.HalfConfirmedContracted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetReserved(PrimeTcrItemForList item)
    {
        return null;
    }

    public SystemPrivileges? CanSetResolved(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsSetResolved;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ReopenDev, (int)PrimeTcrStatuses.ReopenSales };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetReviewing(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanSetReviewing;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.Reserved, (int)PrimeTcrStatuses.Contracted, (int)PrimeTcrStatuses.Resolved, (int)PrimeTcrStatuses.Postponed };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }


    public SystemPrivileges? CanUpdateConfirmationDate(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanUpdateConfirmationDate;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { (int)PrimeTcrStatuses.ConfirmedContracted };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.PrimeTcrStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUpdateCreationDate(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsCanUpdateCreationDate;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var disAllowedStatuses = new[] { (int)PrimeTcrStatuses.Deleted, (int)PrimeTcrStatuses.CanceledByDeveloper };

        if (userPrivilegeItem == null || disAllowedStatuses.Contains(item.PrimeTcrStatusId))
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanVerify(PrimeTcrItemForList item)
    {
        var retVal = SystemPrivileges.PrimeTcrsVerify;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || item.VerificationStatusId != (int)TcrVerificationStatuses.NotYet)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public void PopulateAllowedActions(IEnumerable<PrimeTcrItemForList> items)
    {
        if (currentUser.Privileges.Any(z =>
            z is { PrivilegeCategory: PrivilegeCategories.PrimeTcrs, AccessOnly: false } || z is
            { PrivilegeCategory: PrivilegeCategories.Accounting, AccessOnly: false }) == false)
        {
            return;
        }

        var canMethods = GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name.StartsWith("Can") && m.ReturnType == typeof(SystemPrivileges?));

        foreach (var item in items)
        {
            var enumerable = canMethods as MethodInfo[] ?? canMethods.ToArray();
            var methodInfos = canMethods as MethodInfo[] ?? enumerable.ToArray();
            var allowedActions = new List<SystemPrivileges>();

            // Invoke each "Can" method dynamically
            foreach (var method in methodInfos)
            {
                if (method.Invoke(this, [item]) is SystemPrivileges result)
                {
                    allowedActions.Add(result);
                }
            }

            item.AllowedActions = allowedActions.ToArray();
        }
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }

    protected override void PopulateInitialData()
    {
    }

    private bool PrivilegeInScope(UserPrivilegeItem userPrivilegeItem, PrimeTcrItemForList item)
    {
        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Global)
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Company && currentUser.TeamsIds.Length == 0 && currentUser.CompanyId > 0 &&
            item.CompanyId > 0 && currentUser.CompanyId == item.CompanyId)
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Branch && currentUser.TeamsIds.Length == 0 && currentUser.BranchId > 0 &&
            item.BranchId > 0 && currentUser.BranchId == item.BranchId)
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Own && item.AgentsIdsArray.Length > 0 &&
            item.AgentsIdsArray.Contains(currentUser.UserId))
        {
            return true;
        }

        if ((userPrivilegeItem.PrivilegeScope == PrivilegeScopes.DirectTeam ||
            userPrivilegeItem.PrivilegeScope == PrivilegeScopes.TreeTeam) && currentUser.TeamsIds.Length == 0 && (item.AgentsIdsArray.Contains(currentUser.UserId) || item.ManagersIds.Contains(currentUser.UserId)))
        {
            return true;
        }

        if (currentUser.TeamsIds.Length > 0 && item.AgentsIdsArray.Any(currentUser.TeamsIds.Contains) || item.ManagersIds.Any(currentUser.TeamsIds.Contains))
        {
            return true;
        }

        return false;
    }
}