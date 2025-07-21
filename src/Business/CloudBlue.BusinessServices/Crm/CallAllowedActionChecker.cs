using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.BusinessServices.Crm;

public class CallAllowedActionChecker : ICallAllowedActionChecker
{
    public void PopulateAllowedActions(CallItemForList[] items)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanEdit(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanAssignToAgent(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanReAssignToAgent(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanAssignToBranch(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanReAssignToBranch(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanAddEvent(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanMoveToCompany(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanArchive(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanUnArchive(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public SystemPrivileges? CanCancel(CallItemForList item)
    {
        throw new NotImplementedException();
    }

    public List<Errors> LastErrors { get; set; }
    public long CreateItemId { get; set; }
}