using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DataModels.Tenants;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.App;

public class LookUpsService(ILookupsRepository repo, ICachingService cache, LoggedInUserInfo loggedInUserInfo)
    : BaseService, ILookUpsService
{


    private const int SalesDepartmentId = 1;
    public async Task<LookupItem<int>[]> GetDocumentTypesList()
    {
        var lookupName = "DocumentTypes";
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                new LookupItem<int>("Select One", 0, "", 0),
                new LookupItem<int>("Reservation", 1, "", 0),
                new LookupItem<int>("Contract", 2, "", 0)
            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return await Task.FromResult(retLookups.ToArray());
    }

    public async Task<LookupItem<int>[]> GetAssigningTypes()
    {
        var lookupName = nameof(AssigningTypes);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                new LookupItem<int>("Fresh First Agent", 1, "", 0),
                new LookupItem<int>("Fresh Re-Assigned", 2, "", 0),
                new LookupItem<int>("ReAssigned", 3, "", 0),
                new LookupItem<int>("Reassigned Once", 4, "", 0),
                new LookupItem<int>("Qualified Once", 5, "", 0),
                new LookupItem<int>("Qualified Leads Exceed Two Weeks", 10, "", 0),
                new LookupItem<int>("Assigned Leads Exceed Two Hours", 7, "", 0),
                new LookupItem<int>("No Answer Leads Exceed Two Hours", 8, "", 0),
                new LookupItem<int>("Call Later Leads Exceed Follow-up Date", 9, "", 0),



            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return await Task.FromResult(retLookups.ToArray());
    }

    public bool RefreshMode { get; set; }

    public async Task<LookupItem<int>[]> GetYesNoList(string firstOption = "All")
    {
        var lookupName = "YesNo";
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                new LookupItem<int>(firstOption, 0, "", 0),
                new LookupItem<int>("Yes", 1, "", 0),
                new LookupItem<int>("No", 2, "", 0)
            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return await Task.FromResult(retLookups.ToArray());
    }

    public async Task<LookupItem<int>[]> GetSalesPersonClasses(bool excludeTopManager)
    {
        var lookupName = nameof(SalesPersonClasses);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                new LookupItem<int>("All", 0, "", 0),
                new LookupItem<int>("Top Managers", 3, "", 0),
                new LookupItem<int>("Managers", 2, "", 0),
                new LookupItem<int>("Agents", 1, "", 0),
                new LookupItem<int>("Recs", 4, "", 0)
            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        LookupItem<int>[] retList;

        if (excludeTopManager)
        {
            retList = retLookups.Where(z => z.ItemId != 3)
                .ToArray();
        }
        else
        {
            retList = retLookups;
        }

        return await Task.FromResult(retList);
    }

    public async Task<LookupItem<int>[]> GetCountriesAsync()
    {
        var json = GetLookUpsFromCache(nameof(LookUpCountry));
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (RefreshMode || retLookups.Length == 0)
        {
            retLookups = await repo.GetCountriesAsync();
            SaveLookUpsInCache(nameof(LookUpCountry), UtilityFunctions.SerializeToJsonString(retLookups));

        }


        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetClientTypesAsync()
    {
        var lookupName = nameof(LookUpClientType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetClientTypesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetClientTitlesAsync()
    {
        var lookupName = nameof(LookUpClientTitle);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetClientTitlesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetDeviceTypesAsync()
    {
        var lookupName = nameof(LookUpDeviceType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetDeviceTypesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCallTypesAsync()
    {
        var lookupName = nameof(LookUpCallType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetCallTypesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetWorkFieldsAsync()
    {
        var lookupName = nameof(LookUpWorkField);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetWorkFieldsAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetLeadSourcesAsync(bool applySalesFilter = false)
    {
        var lookupName = nameof(LookUpLeadSource);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups == null || retLookups.Length == 0)
        {
            retLookups = await repo.GetLeadSourcesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }
        if (applySalesFilter && loggedInUserInfo.DepartmentId == SalesDepartmentId)
        {
            return retLookups.Where(z => z.ExtraId == "1").ToArray();
        }

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetKnowItemsAsync(bool applySalesFilter = false)
    {
        var lookupName = nameof(LookUpKnowSource);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {

            retLookups = await repo.GetKnowItemsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        if (applySalesFilter && loggedInUserInfo.DepartmentId == SalesDepartmentId)
        {
            return retLookups.Where(z => z.ExtraId == "1" && z.SubLookUps.Any(x => x.ExtraId == "1")).ToArray();
        }
        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCompaniesAsync()
    {
        var privilege = CheckPrivilege(SystemPrivileges.CompaniesList);

        if (privilege == null)
        {
            return [];
        }

        var companyId = 0;
        var branchId = 0;

        if (privilege.PrivilegeScope != PrivilegeScopes.Global)
        {
            companyId = loggedInUserInfo.CompanyId;
            branchId = loggedInUserInfo.BranchId;
        }

        var lookupName = nameof(FranchiseCompany);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetCompaniesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.Where(z =>
                (companyId == 0 || z.ItemId == companyId) &&
                (branchId == 0 || z.SubLookUps.Any(b => b.ItemId == branchId)))
            .ToArray();
    }

    public async Task<AgentItem[]> GetAgentsAsync(bool activeOnly)
    {
        const int recPositionId = 16;
        var privilege = CheckPrivilege(SystemPrivileges.AgentsList);

        if (privilege == null)
        {
            return [];
        }

        var lastTimeAgentsRefreshed = cache.GetLastAgentsRefreshTime();
        var lookupName = nameof(AgentItem);
        var json = GetLookUpsFromCache(lookupName);
        AgentItem[] retLookups = [];

        if (lastTimeAgentsRefreshed != null && DateTime.UtcNow.Subtract(lastTimeAgentsRefreshed.Value)
               .TotalMinutes < 10 && string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<AgentItem[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetAgentsAsync();

            foreach (var agentItem in retLookups)
            {


                if (agentItem.AgentId == agentItem.TopMostManagerId)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.TopManager;
                }
                else if (agentItem.CanHaveTeam && agentItem.DirectManagerId > 0 && agentItem.TopMostManagerId > 0)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Manager;
                }
                else if (agentItem.PositionId == recPositionId)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Rec;
                }
                else
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Agent;
                }
            }

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
            cache.SaveLastAgentsRefreshTime();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Global)
        {
            return retLookups.Where(z => activeOnly == false || z.IsApproved).OrderBy(z => z.AgentName)
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Company && loggedInUserInfo.TeamsIds.Length == 0)
        {
            return retLookups.Where(z => z.CompanyId == loggedInUserInfo.CompanyId && (activeOnly == false || z.IsApproved))
                .OrderBy(z => z.AgentName)
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch && loggedInUserInfo.TeamsIds.Length == 0)
        {
            return retLookups.Where(z => z.BranchId == loggedInUserInfo.BranchId && (activeOnly == false || z.IsApproved))
                .OrderBy(z => z.AgentName)
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.DirectTeam)
        {
            return retLookups.Where(z =>
                    (z.DirectManagerId == loggedInUserInfo.UserId || z.AgentId == loggedInUserInfo.UserId) && (activeOnly == false || z.IsApproved))
                .OrderBy(z => z.AgentName)
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.TreeTeam ||
           loggedInUserInfo.TeamsIds.Length > 0)
        {
            var teamsIds = new List<int>();
            teamsIds.Add(loggedInUserInfo.UserId);

            if (loggedInUserInfo.TeamsIds.Length > 0)
            {
                teamsIds.AddRange(loggedInUserInfo.TeamsIds);
            }

            if (teamsIds.Count == 0)
            {
                return [];
            }

            var lst = new List<AgentItem>();

            var currentAgents = retLookups.Where(z =>
                    teamsIds.Contains(z.AgentId) || (loggedInUserInfo.UserGroupId == (int)UserDomains.Branch &&
                                                     z.AgentId == loggedInUserInfo.UserId))
                .ToArray();

            if (currentAgents.Length == 0)
            {
                return [];
            }

            lst.AddRange(currentAgents);

            for (var i = 0; i < currentAgents.Length; i++)
            {
                PopulateAgentListRecursively(lst, retLookups.ToList(), currentAgents[i].AgentId);
            }

            return lst.Where(z => (activeOnly == false || z.IsApproved)).ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            return retLookups.Where(z => z.AgentId == loggedInUserInfo.UserId)
                .OrderBy(z => z.AgentName)
                .ToArray();
        }

        return [];
    }

    public async Task<AgentItem[]> GetSalesPersonsAsync(SalesPersonClasses[] salesPersonClasses, bool activeOnly)
    {
        var lookupName = nameof(AgentItem);
        var json = GetLookUpsFromCache(lookupName);
        AgentItem[] retLookups = [];
        var recPositionId = 16;

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<AgentItem[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetAgentsAsync();

            foreach (var agentItem in retLookups)
            {
                if (agentItem.AgentId == agentItem.TopMostManagerId)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.TopManager;
                }
                else if (agentItem.CanHaveTeam && agentItem.DirectManagerId > 0 && agentItem.TopMostManagerId > 0)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Manager;
                }
                else if (agentItem.PositionId == recPositionId)
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Rec;

                }
                else
                {
                    agentItem.SalesPersonClass = SalesPersonClasses.Agent;
                }
            }

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.Where(z => (salesPersonClasses.Length == 0 || salesPersonClasses.Contains(z.SalesPersonClass)) && (activeOnly == false || z.IsApproved))
            .OrderBy(z => z.AgentName)
            .ToArray();
    }

    public async Task<LookupItem<int>[]> GetUsageAsync(int usageId = 0)
    {
        var lookupName = nameof(LookUpUsage);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetUsagesAsync(0, 0);
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.Where(z => usageId == 0 || z.ItemId == usageId)
            .ToArray();
    }

    public async Task<LookupItem<int>[]> GetSalesTypesAsync()
    {
        var lookupName = nameof(LookUpSalesType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetSalesTypesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetServicesAsync()
    {
        var lookupName = nameof(LookUpService);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetServicesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCurrenciesAsync()
    {
        var lookupName = nameof(LookUpCurrency);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetCurrenciesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetGendersAsync()
    {
        var lookupName = nameof(LookUpGender);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetGendersAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetClientCategoriesAsync()
    {
        var lookupName = nameof(LookUpClientCategory);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetClientCategoriesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCallStatusesAsync()
    {
        var lookupName = nameof(LookUpCallStatus);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetCallStatusesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCallRecipientsAsync()
    {
        var privilege = CheckPrivilege(SystemPrivileges.CallsManage);

        if (privilege == null)
        {
            return [];
        }

        var lookupName = nameof(VwCallRecipient);
        var json = GetLookUpsFromCache(lookupName);
        UserBriefInfo[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<UserBriefInfo[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetCallRecipientsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Global)
        {
            return retLookups.OrderBy(z => z.UserFullName)
                .Select(z => new LookupItem<int>(lookupName = z.UserFullName, z.UserId, "", 0))
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Company)
        {
            return retLookups.Where(z =>
                    z.UserGroupId == (int)UserDomains.CallCenter || z.CompanyId == loggedInUserInfo.CompanyId)
                .OrderBy(z => z.UserFullName)
                .Select(z => new LookupItem<int>(lookupName = z.UserFullName, z.UserId, "", 0))
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Branch)
        {
            return retLookups.Where(z =>
                    z.UserGroupId == (int)UserDomains.CallCenter || z.BranchId == loggedInUserInfo.BranchId)
                .OrderBy(z => z.UserFullName)
                .Select(z => new LookupItem<int>(lookupName = z.UserFullName, z.UserId, "", 0))
                .ToArray();
        }

        if (privilege.PrivilegeScope == PrivilegeScopes.Own)
        {
            return retLookups.Where(z => z.UserId == loggedInUserInfo.UserId)
                .OrderBy(z => z.UserFullName)
                .Select(z => new LookupItem<int>(lookupName = z.UserFullName, z.UserId, "", 0))
                .ToArray();
        }

        return [];
    }

    public async Task<LookupItem<int>[]> GetMarketingAgenciesAsync()
    {
        var lookupName = nameof(MarketingAgency);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (RefreshMode || retLookups.Length == 0)
        {

            retLookups = await repo.GetMarketingAgenciesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetLeadTicketStatusesAsync()
    {
        var lookupName = nameof(LookUpBlindLeadTicketStatus);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetLeadTicketStatusesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetGracePeriodsAsync()
    {
        var lookupName = nameof(LookUpGracePeriod);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];
#pragma warning disable RCS1049FadeOut
        if (string.IsNullOrEmpty(json) == false)
#pragma warning restore RCS1049FadeOut
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetGracePeriodsAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetCorporateCompaniesAsync()
    {
        var lookupName = nameof(LookupCorporateCompany);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetCorporateCompaniesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<PrivilegeItem[]> GetSystemPrivilegesAsync()
    {
        var lookupName = nameof(PrivilegeItem);
        var json = GetLookUpsFromCache(lookupName);
        PrivilegeItem[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<PrivilegeItem[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetSystemPrivilegesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<ContactingTypeItem[]> GetContactingTypesAsync()
    {
        var lookupName = nameof(ContactingTypeItem);
        var json = GetLookUpsFromCache(lookupName);
        ContactingTypeItem[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<ContactingTypeItem[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetContactingTypesAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetVoidReasonsAsync()
    {
        var lookupName = nameof(LookUpVoidReason);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups is { Length: > 0 })
        {
            return retLookups;
        }

        retLookups = await repo.GetVoidReasonsAsync();
        SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));

        return retLookups;
    }

    public async Task<LookupItem<int>[]> GetPropertyTypesAsync(int usageId)
    {
        var lookupName = nameof(LookUpPropertyType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetPropertyTypesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.Where(z => usageId == 0 || z.ParentItemId == usageId)
            .ToArray();
    }

    public async Task<LookupItem<int>[]> GetPrivilegesAsync()
    {
        var lookupName = nameof(Privilege);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetPrivilegesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<LookupItem<int>[]> GetDepartmentsAsync()
    {
        var lookupName = nameof(Department);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetDepartmentsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<LookupItem<int>[]> GetUserPositionsAsync()
    {
        var lookupName = nameof(UserPosition);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetUserPositionsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<LookupItem<int>[]> GetPrivilegeCategoriesAsync()
    {
        var lookupName = nameof(PrivilegeCategory);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetPrivilegeCategoriesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<LookupItem<int>[]> GetEntityTypesAsync()
    {
        var lookupName = nameof(PrivilegeEntityType);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetEntityTypesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<LookupItem<int>[]> GetPrivilegeScopesAsync()
    {
        var lookupName = nameof(PrivilegeScope);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetPrivilegeScopesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<IEnumerable<ActiveUserItem>> GetActiveUsersAsync()
    {
        var lookupName = nameof(ActiveUserItem);
        var json = GetLookUpsFromCache(lookupName);
        ActiveUserItem[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<ActiveUserItem[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetActiveUsersAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<IEnumerable<LookupItem<int>>> GetOutsideBrokersAsync()
    {
        var lookupName = nameof(LookUpOutsideBroker);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetOutsideBrokersAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<IEnumerable<LookupItem<int>>> GetDevelopersAsync()
    {
        var lookupName = nameof(ConstructionDeveloper);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetDevelopersAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<IEnumerable<LookupItem<int>>> GetVerificationStatusesAsync()
    {
        var lookupName = nameof(TcrVerificationStatuses);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                new LookupItem<int>("All", -1, "", 0),
                new LookupItem<int>("Not Yet", 0, "", 0),
                new LookupItem<int>("Verified", 1, "", 0),
                new LookupItem<int>("Doubted", 2, "", 0)
            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return await Task.FromResult(retLookups.ToArray());
    }

    public async Task<IEnumerable<LookupItem<int>>> GetPrimeTcrStatusesAsync()
    {
        var lookupName = nameof(LookUpPrimeTcrStatus);
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetPrimeTcrStatusesAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups.ToArray();
    }

    public async Task<DashboardNotificationItem[]> GetDashboardNotificationsAsync()
    {
        var lookupName = nameof(DashboardNotificationItem);
        var json = GetLookUpsFromCache(lookupName);
        DashboardNotificationItem[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<DashboardNotificationItem[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups = await repo.GetDashboardNotificationsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups;
    }

    public async Task<IEnumerable<LocationItem>> GetLocationsAsync()
    {
        var lookupName = nameof(LocationItem);
        var json = GetLookUpsFromCache(lookupName);
        LocationItem[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LocationItem[]>(json);
        }

        if (RefreshMode || retLookups.Length == 0)
        {

            retLookups = await repo.GetLocationsAsync();
            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return retLookups;

    }

    public async Task<IEnumerable<LookupItem<int>>> GetNeighborhoodTypesAsync()
    {
        var lookupName = "NeighborhoodTypes";
        var json = GetLookUpsFromCache(lookupName);
        LookupItem<int>[] retLookups = [];

        if (string.IsNullOrEmpty(json) == false)
        {
            retLookups = UtilityFunctions.DeserializeJsonString<LookupItem<int>[]>(json);
        }

        if (retLookups.Length == 0)
        {
            retLookups =
            [
                 new LookupItem<int>("Neighborhood", 1, "", 0),
                new LookupItem<int>("Project", 2, "", 0)
            ];

            SaveLookUpsInCache(lookupName, UtilityFunctions.SerializeToJsonString(retLookups));
        }

        return await Task.FromResult(retLookups.ToArray());

    }

    private void PopulateAgentListRecursively(List<AgentItem> lst, List<AgentItem> retLookups, int userId)
    {
        var agents = retLookups.Where(z => z.DirectManagerId == userId)
            .ToList();

        if (agents.Count == 0)
        {
            return;
        }

        lst.AddRange(agents);

        foreach (var agent in agents)
        {
            PopulateAgentListRecursively(lst, retLookups, agent.AgentId);
        }
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

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    private void SaveLookUpsInCache(string lookUpName, string serializeJson)
    {
        cache.SaveItem(lookUpName, serializeJson);
    }

    private string GetLookUpsFromCache(string lookUpName)
    {
        return cache.GetItem(lookUpName);
    }
}