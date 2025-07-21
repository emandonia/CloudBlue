using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ILookUpsService : IBaseService
{
    bool RefreshMode { set; get; }
    Task<LookupItem<int>[]> GetYesNoList(string firstOption = "All");
    Task<LookupItem<int>[]> GetDocumentTypesList();
    Task<LookupItem<int>[]> GetSalesPersonClasses(bool excludeTopManager);
    Task<LookupItem<int>[]> GetAssigningTypes();
    Task<LookupItem<int>[]> GetCountriesAsync();
    Task<LookupItem<int>[]> GetClientTypesAsync();
    Task<LookupItem<int>[]> GetClientTitlesAsync();
    Task<LookupItem<int>[]> GetDeviceTypesAsync();
    Task<LookupItem<int>[]> GetCallTypesAsync();
    Task<LookupItem<int>[]> GetWorkFieldsAsync();
    Task<LookupItem<int>[]> GetLeadSourcesAsync(bool applySalesFilter = false);
    Task<LookupItem<int>[]> GetKnowItemsAsync(bool applySalesFilter = false);
    Task<LookupItem<int>[]> GetCompaniesAsync();
    Task<AgentItem[]> GetAgentsAsync(bool activeOnly);
    Task<AgentItem[]> GetSalesPersonsAsync(SalesPersonClasses[] salesPersonClass, bool activeOnly);
    Task<LookupItem<int>[]> GetUsageAsync(int usageId = 0);
    Task<LookupItem<int>[]> GetSalesTypesAsync();
    Task<LookupItem<int>[]> GetServicesAsync();
    Task<LookupItem<int>[]> GetCurrenciesAsync();
    Task<LookupItem<int>[]> GetGendersAsync();
    Task<LookupItem<int>[]> GetClientCategoriesAsync();
    Task<LookupItem<int>[]> GetCallStatusesAsync();
    Task<LookupItem<int>[]> GetCallRecipientsAsync();
    Task<LookupItem<int>[]> GetMarketingAgenciesAsync();
    Task<LookupItem<int>[]> GetLeadTicketStatusesAsync();
    Task<LookupItem<int>[]> GetGracePeriodsAsync();
    Task<LookupItem<int>[]> GetCorporateCompaniesAsync();
    Task<PrivilegeItem[]> GetSystemPrivilegesAsync();
    Task<ContactingTypeItem[]> GetContactingTypesAsync();
    Task<LookupItem<int>[]> GetVoidReasonsAsync();
    Task<LookupItem<int>[]> GetPropertyTypesAsync(int usageId);
    Task<LookupItem<int>[]> GetPrivilegesAsync();
    Task<LookupItem<int>[]> GetDepartmentsAsync();
    Task<LookupItem<int>[]> GetUserPositionsAsync();
    Task<LookupItem<int>[]> GetPrivilegeCategoriesAsync();
    Task<LookupItem<int>[]> GetEntityTypesAsync();
    Task<LookupItem<int>[]> GetPrivilegeScopesAsync();

    Task<IEnumerable<ActiveUserItem>> GetActiveUsersAsync();
    Task<IEnumerable<LookupItem<int>>> GetOutsideBrokersAsync();
    Task<IEnumerable<LookupItem<int>>> GetDevelopersAsync();
    Task<IEnumerable<LookupItem<int>>> GetVerificationStatusesAsync();
    Task<IEnumerable<LookupItem<int>>> GetPrimeTcrStatusesAsync();
    Task<DashboardNotificationItem[]> GetDashboardNotificationsAsync();
    Task<IEnumerable<LocationItem>> GetLocationsAsync();
    Task<IEnumerable<LookupItem<int>>> GetNeighborhoodTypesAsync();
}