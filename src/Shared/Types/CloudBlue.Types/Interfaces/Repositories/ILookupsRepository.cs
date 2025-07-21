using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface ILookupsRepository : IBaseRepository
{
    Task<LookupItem<int>[]> GetPrivilegesAsync();
    Task<LookupItem<int>[]> GetDepartmentsAsync();
    Task<LookupItem<int>[]> GetUserPositionsAsync();
    Task<LookupItem<int>[]> GetPrivilegeCategoriesAsync();
    Task<LookupItem<int>[]> GetEntityTypesAsync();
    Task<LookupItem<int>[]> GetPrivilegeScopesAsync();

    Task<LookupItem<int>[]> GetCountriesAsync();
    Task<LookupItem<int>[]> GetClientTypesAsync();
    Task<LookupItem<int>[]> GetClientTitlesAsync();
    Task<LookupItem<int>[]> GetDeviceTypesAsync();
    Task<LookupItem<int>[]> GetCallTypesAsync();
    Task<LookupItem<int>[]> GetWorkFieldsAsync();

    Task<LookupItem<int>[]> GetKnowItemsAsync();
    Task<LookupItem<int>[]> GetCompaniesAsync();
    Task<LookupItem<int>[]> GetUsagesAsync(int companyId, int branchId);
    Task<LookupItem<int>[]> GetSalesTypesAsync();
    Task<LookupItem<int>[]> GetServicesAsync();
    Task<LookupItem<int>[]> GetCurrenciesAsync();
    Task<LookupItem<int>[]> GetLeadSourcesAsync();
    Task<AgentItem[]> GetAgentsAsync();
    Task<LookupItem<int>[]> GetGendersAsync();
    Task<LookupItem<int>[]> GetClientCategoriesAsync();
    Task<LookupItem<int>[]> GetCallStatusesAsync();
    Task<UserBriefInfo[]> GetCallRecipientsAsync();
    Task<LookupItem<int>[]> GetMarketingAgenciesAsync();
    Task<LookupItem<int>[]> GetLeadTicketStatusesAsync();
    Task<LookupItem<int>[]> GetGracePeriodsAsync();
    Task<LookupItem<int>[]> GetCorporateCompaniesAsync();
    Task<PrivilegeItem[]> GetSystemPrivilegesAsync();
    Task<ContactingTypeItem[]> GetContactingTypesAsync();
    Task<LookupItem<int>[]> GetVoidReasonsAsync();
    Task<LookupItem<int>[]> GetPropertyTypesAsync();

    Task<ActiveUserItem[]> GetActiveUsersAsync();
    Task<LookupItem<int>[]> GetOutsideBrokersAsync();
    Task<LookupItem<int>[]> GetDevelopersAsync();
    Task<LookupItem<int>[]> GetPrimeTcrStatusesAsync();
    Task<DashboardNotificationItem[]> GetDashboardNotificationsAsync();
    Task<LocationItem[]> GetLocationsAsync();
}