using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.Domain.BaseTypes;

public abstract class BaseService : IBaseService

{
    protected BaseService()
    {
        Initialize();
    }

    public List<Errors> LastErrors { get; set; } = new();
    public long CreateItemId { get; set; }

    private void Initialize()
    {
        PopulateInitialData();
    }

    protected abstract void PopulateInitialData();

    protected abstract UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege);
}