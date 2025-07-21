using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ICallAllowedActionChecker : IAllowedActionChecker<CallItemForList>
{
    SystemPrivileges? CanCancel(CallItemForList item);
}