using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IAllowedActionChecker<T> : IBaseService
{
    void PopulateAllowedActions(T[] items);
    SystemPrivileges? CanEdit(T item);
    SystemPrivileges? CanAssignToAgent(T item);
    SystemPrivileges? CanReAssignToAgent(T item);
    SystemPrivileges? CanAssignToBranch(T item);
    SystemPrivileges? CanReAssignToBranch(T item);
    SystemPrivileges? CanAddEvent(T item);
    SystemPrivileges? CanMoveToCompany(T item);
    SystemPrivileges? CanArchive(T item);
    SystemPrivileges? CanUnArchive(T item);
}