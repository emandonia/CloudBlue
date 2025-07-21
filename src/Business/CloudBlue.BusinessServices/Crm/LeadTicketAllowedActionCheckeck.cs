using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;
using System.Reflection;

namespace CloudBlue.BusinessServices.Crm;

public class LeadTicketAllowedActionChecker(LoggedInUserInfo currentUser) : BaseService, ILeadTicketAllowedActionChecker
{
    public void PopulateAllowedActions(LeadTicketItemForList[] items)
    {
        if (currentUser.Privileges.Any(z => z is
            { PrivilegeCategory: PrivilegeCategories.LeadTickets, AccessOnly: false }) == false)
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

    public SystemPrivileges? CanEdit(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsEdit;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[]
        {
            LeadTicketStatuses.Unassigned, LeadTicketStatuses.InProgress, LeadTicketStatuses.Assigned
        };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    private const int SalesDepartmentId = 1;

    public SystemPrivileges? CanAssignToAgent(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsAssignToAgent;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { LeadTicketStatuses.Unassigned, LeadTicketStatuses.Assigned };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false)
        {
            return null;
        }

        if (currentUser.DepartmentId == SalesDepartmentId && item.LeadTicketStatusId != LeadTicketStatuses.Unassigned)
        {
            return null;
        }

        if (currentUser.DepartmentId != SalesDepartmentId && item.LeadTicketStatusId == LeadTicketStatuses.Assigned &&
           item.CurrentAgentId != currentUser.UserId)
        {
            return null;
        }



        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanReAssignToAgent(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsReAssignToAgent;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var notAllowedStatuses = new[] { LeadTicketStatuses.Unassigned };

        if (userPrivilegeItem == null || notAllowedStatuses.Contains(item.LeadTicketStatusId))
        {
            return null;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Own ||
           PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAssignToBranch(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsAssignToBranch;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { LeadTicketStatuses.Unassigned };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false || item.BranchId > 0)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanReAssignToBranch(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsReAssignToBranch;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[]
        {
            LeadTicketStatuses.Unassigned, LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress
        };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false ||
           item.BranchId == 0)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddEvent(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsAddEvent;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || item.LeadTicketStatusId == LeadTicketStatuses.Assigned || PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanConvertToPrimeTcr(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsConvertToPrimeTcr;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false ||
           item.SalesTypeId != SalesTypes.Prime || item.ServiceId != LeadTicketServices.Buy)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanConvertToBuyerRequest(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsConvertToBuyerRequest;
        var allowedServices = new[] { LeadTicketServices.Buy, LeadTicketServices.RentBuyer };
        var allowedStatuses = new[] { LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress };
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || item.SalesTypeId == SalesTypes.Prime ||
           allowedServices.Contains(item.ServiceId) == false ||
           allowedStatuses.Contains(item.LeadTicketStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanConvertToSellerRequest(LeadTicketItemForList item)
    {
        var allowedServices = new[] { LeadTicketServices.Sell, LeadTicketServices.RentSeller };
        var retVal = SystemPrivileges.LeadTicketsConvertToSellerRequest;
        var allowedStatuses = new[] { LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress };
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false ||
           item.SalesTypeId == SalesTypes.Prime || allowedServices.Contains(item.ServiceId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanReject(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsReject;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanSetVoid(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsSetVoid;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[]
        {
            LeadTicketStatuses.Unassigned, LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress
        };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false || item.IsVoided)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanAddFeedBackEvent(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsAddFeedback;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || (item.CurrentAgentId != currentUser.UserId))
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanArchive(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsArchive;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || item.IsArchived)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanUnArchive(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsUnArchive;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        if (userPrivilegeItem == null || item.IsArchived == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanMoveToCompany(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsMoveToCompany;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);

        var allowedStatuses = new[]
        {
            LeadTicketStatuses.Unassigned, LeadTicketStatuses.Assigned, LeadTicketStatuses.InProgress
        };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    public SystemPrivileges? CanCreateDuplicate(LeadTicketItemForList item)
    {
        var retVal = SystemPrivileges.LeadTicketsCreateDuplicate;
        var userPrivilegeItem = currentUser.Privileges.FirstOrDefault(z => z.Privilege == retVal);
        var allowedStatuses = new[] { LeadTicketStatuses.Tcr };

        if (userPrivilegeItem == null || allowedStatuses.Contains(item.LeadTicketStatusId) == false ||
           item.IsClosed == false)
        {
            return null;
        }

        if (PrivilegeInScope(userPrivilegeItem, item) == false)
        {
            return null;
        }

        return retVal;
    }

    private bool PrivilegeInScope(UserPrivilegeItem userPrivilegeItem, LeadTicketItemForList item)
    {
        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Global)
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Company && currentUser.CompanyId > 0 &&
           item.CompanyId > 0 && currentUser.CompanyId == item.CompanyId)
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Branch && currentUser.BranchId > 0 &&
           item.BranchId > 0 && currentUser.BranchId == item.BranchId)
        {
            return true;
        }

        if ((userPrivilegeItem.PrivilegeScope == PrivilegeScopes.DirectTeam ||
            userPrivilegeItem.PrivilegeScope == PrivilegeScopes.TreeTeam)
            &&

            (item.CurrentAgentId > 0 &&
             (
                 (currentUser.AgentsIds.Length > 0 &&
             currentUser.AgentsIds.Contains(item.CurrentAgentId))

             ||
                 (item.ManagersIdsArray != null && item.ManagersIdsArray.Length > 0 && item.ManagersIdsArray.Contains(currentUser.UserId)))))
        {
            return true;
        }

        if (userPrivilegeItem.PrivilegeScope == PrivilegeScopes.Own && item.CurrentAgentId > 0 &&
           currentUser.UserId == item.CurrentAgentId)
        {
            return true;
        }

        return false;
    }

    protected override void PopulateInitialData()
    {
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }
}