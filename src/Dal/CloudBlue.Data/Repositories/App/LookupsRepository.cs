using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.DashboardStuff;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.Repositories.App;

public class LookupsRepository(ILookUpsDataContext lookupsDb) : ILookupsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public async Task<LookupItem<int>[]> GetPrivilegesAsync()
    {
        var lookups = await lookupsDb.Privileges.AsNoTracking()
            .OrderBy(z => z.PrivilegeName)
            .Select(z => new LookupItem<int>(z.PrivilegeName, z.Id, string.Empty, z.PrivilegeCategoryId))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetDepartmentsAsync()
    {
        var lookups = await lookupsDb.Departments.AsNoTracking()
            .OrderBy(z => z.DepartmentName)
            .Select(z => new LookupItem<int>(z.DepartmentName, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetUserPositionsAsync()
    {
        var lookups = await lookupsDb.UserPositions.AsNoTracking()
            .OrderBy(z => z.UserPositionName)
            .Select(z => new LookupItem<int>(z.UserPositionName, z.Id, string.Empty, z.DepartmentId))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetPrivilegeCategoriesAsync()
    {
        var lookups = await lookupsDb.PrivilegeCategories.AsNoTracking()
            .OrderBy(z => z.PrivilegeCategoryName)
            .Select(z => new LookupItem<int>(z.PrivilegeCategoryName, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetEntityTypesAsync()
    {
        var lookups = await lookupsDb.PrivilegeEntityTypes.AsNoTracking()
            .OrderBy(z => z.PrivilegeEntityTypeName)
            .Select(z => new LookupItem<int>(z.PrivilegeEntityTypeName, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetPrivilegeScopesAsync()
    {
        var lookups = await lookupsDb.PrivilegeScopes.AsNoTracking()
            .OrderBy(z => z.PrivilegeScopeName)
            .Select(z => new LookupItem<int>(z.PrivilegeScopeName, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookups;
    }

    public async Task<LookupItem<int>[]> GetCountriesAsync()
    {
        var rawCountries = await lookupsDb.LookUpCountries.Include(z => z.LookUpCities)
            .ThenInclude(z => z.LookUpDistricts)
            .ThenInclude(z => z.LookUpNeighborhoods)
            .AsNoTracking()
            .ToArrayAsync();

        var countries = new List<LookupItem<int>>();

        foreach (var rawCountry in rawCountries.OrderBy(z => z.DisplayOrder)
                    .ThenBy(z => z.Country))
        {
            var country = new LookupItem<int>($"{rawCountry.Country}", rawCountry.Id, rawCountry.PhoneCode, 0);
            var cities = new List<LookupItem<int>>();

            foreach (var lookUpCity in rawCountry.LookUpCities.OrderBy(z => z.City))
            {
                var city = new LookupItem<int>(lookUpCity.City, lookUpCity.Id, "", rawCountry.Id);
                var districts = new List<LookupItem<int>>();

                foreach (var lookUpDistrict in lookUpCity.LookUpDistricts.OrderBy(z => z.District))
                {
                    var district = new LookupItem<int>(lookUpDistrict.District, lookUpDistrict.Id, "", lookUpCity.Id);
                    var neighborhoods = new List<LookupItem<int>>();

                    foreach (var lookUpNeighborhood in lookUpDistrict.LookUpNeighborhoods.OrderBy(z => z.Neighborhood))
                    {
                        var neighborhood = new LookupItem<int>(lookUpNeighborhood.Neighborhood, lookUpNeighborhood.Id,
                            "", lookUpDistrict.Id);

                        neighborhoods.Add(neighborhood);
                    }

                    district.SubLookUps = neighborhoods.ToArray();
                    districts.Add(district);
                }

                city.SubLookUps = districts.ToArray();
                cities.Add(city);
            }

            country.SubLookUps = cities.ToArray();
            countries.Add(country);
        }

        return countries.ToArray();
    }

    public async Task<LookupItem<int>[]> GetClientTypesAsync()
    {
        var clientTypes = await lookupsDb.LookUpClientTypes.AsNoTracking()
            .Select(z => new LookupItem<int>(z.ClientType, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return clientTypes;
    }

    public async Task<LookupItem<int>[]> GetClientTitlesAsync()
    {
        var clientTitles = await lookupsDb.LookUpClientTitles.OrderBy(z => z.ClientTitle)
            .AsNoTracking()
            .Select(z => new LookupItem<int>(z.ClientTitle, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return clientTitles;
    }

    public async Task<LookupItem<int>[]> GetDeviceTypesAsync()
    {
        var deviceTypes = await lookupsDb.LookUpDeviceTypes.AsNoTracking()
            .Select(z => new LookupItem<int>(z.DeviceType, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return deviceTypes;
    }

    public async Task<LookupItem<int>[]> GetCallTypesAsync()
    {
        var callTypes = await lookupsDb.LookUpCallTypes.OrderBy(z => z.DisplayOrder)
            .AsNoTracking()
            .Select(z => new LookupItem<int>(z.CallType, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return callTypes;
    }

    public async Task<LookupItem<int>[]> GetWorkFieldsAsync()
    {
        var workFields = await lookupsDb.LookUpWorkFields.OrderBy(z => z.WorkField)
            .AsNoTracking()
            .Select(z => new LookupItem<int>(z.WorkField, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return workFields;
    }

    public async Task<LookupItem<int>[]> GetKnowItemsAsync()
    {
        var rawKnowItems = await lookupsDb.LookUpKnowSources.Include(z => z.KnowSubSources)
            .OrderBy(z => z.KnowSource)
            .AsNoTracking()
            .ToArrayAsync();

        var knowItems = new LookupItem<int>[rawKnowItems.Length];

        for (var idx = 0; idx < rawKnowItems.Length; idx++)
        {
            var rawKnowItem = rawKnowItems[idx];
            var knowItem = new LookupItem<int>(rawKnowItem.KnowSource, rawKnowItem.Id, rawKnowItem.ForSalesOnly ? "1" : "0", 0);

            knowItem.SubLookUps = rawKnowItem.KnowSubSources.OrderBy(z => z.KnowSubSource)
                .Select(z => new LookupItem<int>(z.KnowSubSource, z.Id, z.ForSalesOnly ? "1" : "0", z.KnowSourceId))
                .ToArray();

            knowItems.SetValue(knowItem, idx);
        }

        return knowItems;
    }

    public async Task<LookupItem<int>[]> GetCompaniesAsync()
    {
        var rawCompanies = await lookupsDb.Companies.Where(z => z.Branches.Any(x => x.Disabled == false))
            .Include(z => z.Branches)
            .OrderBy(z => z.CompanyName)
            .AsNoTracking()
            .ToArrayAsync();

        var companies = new LookupItem<int>[rawCompanies.Length];

        for (var idx = 0; idx < rawCompanies.Length; idx++)
        {
            var rawCompany = rawCompanies[idx];
            var company = new LookupItem<int>(rawCompany.CompanyName, rawCompany.Id, "", 0);

            company.SubLookUps = rawCompany.Branches.OrderBy(z => z.BranchName)
                .Select(z => new LookupItem<int>(z.BranchName, z.Id, "", z.CompanyId))
                .ToArray();

            companies.SetValue(company, idx);
        }

        return companies;
    }

    public async Task<LookupItem<int>[]> GetUsagesAsync(int companyId, int branchId)
    {
        var usageQ = lookupsDb.LookUpUsages.Include(z => z.LookUpPropertyTypes)
            .OrderBy(z => z.Usage)
            .AsNoTracking();

        if (companyId > 0)
        {
            usageQ = usageQ.Where(z =>
                z.UsageToEntities.Any(x => x.EntityId == companyId && x.EntityType == EntityTypes.Company));
        }
        else if (branchId > 0)
        {
            usageQ = usageQ.Where(z =>
                z.UsageToEntities.Any(x => x.EntityId == branchId && x.EntityType == EntityTypes.Branch));
        }

        var rawUsages = await usageQ.ToArrayAsync();
        var usages = new LookupItem<int>[rawUsages.Length];

        for (var idx = 0; idx < rawUsages.Length; idx++)
        {
            var rawUsage = rawUsages[idx];
            var usage = new LookupItem<int>(rawUsage.Usage, rawUsage.Id, rawUsage.Note, 0);

            var propertyTypes = rawUsage.LookUpPropertyTypes.OrderBy(z => z.PropertyType)
                .Select(z => new LookupItem<int>(z.PropertyType, z.Id, string.Empty, rawUsage.Id))
                .ToArray();

            usage.SubLookUps = propertyTypes;
            usages.SetValue(usage, idx);
        }

        return usages;
    }

    public async Task<LookupItem<int>[]> GetSalesTypesAsync()
    {
        var salesTypes = await lookupsDb.LookUpSalesTypes.AsNoTracking()
            .Select(z => new LookupItem<int>(z.SalesType, z.Id, string.Empty, 0))
            .ToListAsync();

        return salesTypes.ToArray();
    }

    public async Task<LookupItem<int>[]> GetServicesAsync()
    {
        var services = await lookupsDb.LookUpServices.OrderBy(z => z.DisplayOrder)
            .AsNoTracking()
            .Select(z => new LookupItem<int>(z.Service, z.Id, z.Note, 0))
            .ToListAsync();

        return services.ToArray();
    }

    public async Task<LookupItem<int>[]> GetCurrenciesAsync()
    {
        var currencies = await lookupsDb.LookUpCurrencies.AsNoTracking()
            .Select(z => new LookupItem<int>(z.Currency, z.Id, z.CurrencySymbol, 0))
            .ToArrayAsync();

        return currencies;
    }

    public async Task<LookupItem<int>[]> GetLeadSourcesAsync()
    {
        var leadSources = await lookupsDb.LookUpLeadSources.AsNoTracking()
            .OrderBy(z => z.LeadSource)
            .Select(z => new LookupItem<int>(z.LeadSource, z.Id, z.ForSalesOnly ? "1" : "0", 0))
            .ToArrayAsync();

        return leadSources;
    }

    public async Task<AgentItem[]> GetAgentsAsync()
    {
        var agents = await lookupsDb.VwAgents.AsNoTracking()
            .OrderBy(z => z.AgentName)
            .Select(z => new AgentItem
            {
                AgentId = z.AgentId,
                DirectManagerId = z.DirectManagerId,
                TopMostManagerId = z.TopMostManagerId,
                BranchId = z.BranchId,
                CompanyId = z.CompanyId,
                AgentEmail = z.Email,
                AgentName = z.AgentName,
                PositionId = z.PositionId,
                CanHaveTeam = z.CanHaveTeam,
                InResaleTeam = z.InResaleTeam,
                IsApproved = z.IsApproved,
                ManagersIdsArray = z.ManagersIdsArray,
                TeamsIds = z.TeamsIds,
                BranchName = z.BranchName,
                AgentPhone = z.DeviceInfo

            })
            .ToArrayAsync();

        return agents;
    }

    public async Task<LookupItem<int>[]> GetGendersAsync()
    {
        var genders = await lookupsDb.LookUpGenders.AsNoTracking()
            .OrderBy(z => z.Gender)
            .Select(z => new LookupItem<int>(z.Gender, z.Id, string.Empty, 0))
            .ToListAsync();

        return genders.ToArray();
    }

    public async Task<LookupItem<int>[]> GetClientCategoriesAsync()
    {
        var lookupItems = await lookupsDb.LookUpClientCategories.AsNoTracking()
            .Select(z => new LookupItem<int>(z.ClientCategory, z.Id, string.Empty, 0))
            .ToListAsync();

        return lookupItems.ToArray();
    }

    public async Task<LookupItem<int>[]> GetCallStatusesAsync()
    {
        var lookupItems = await lookupsDb.LookUpCallStatuses.AsNoTracking()
            .Select(z => new LookupItem<int>(z.CallStatus, z.Id, string.Empty, 0))
            .ToListAsync();

        return lookupItems.ToArray();
    }

    public async Task<UserBriefInfo[]> GetCallRecipientsAsync()
    {
        var lookupItems = await lookupsDb.VwCallRecipients.AsNoTracking()
            .OrderBy(z => z.UserFullName)
            .Select(z => new UserBriefInfo
            {
                BranchId = z.BranchId,
                CompanyId = z.CompanyId,
                PositionId = z.PositionId,
                UserFullName = z.UserFullName,
                UserGroupId = z.UserGroupId,
                UserId = z.UserId
            })
            .ToListAsync();

        return lookupItems.ToArray();
    }

    public async Task<LookupItem<int>[]> GetMarketingAgenciesAsync()
    {
        var lookupItems = await lookupsDb.MarketingAgencies.AsNoTracking()
            .OrderBy(z => z.Agency)
            .Select(z => new LookupItem<int>(z.Agency, z.Id, string.Empty, 0))
            .ToListAsync();

        return lookupItems.ToArray();
    }

    public async Task<LookupItem<int>[]> GetLeadTicketStatusesAsync()
    {
        var lookupItems = await lookupsDb.LookUpLeadTicketStatuses.AsNoTracking()
            .OrderBy(z => z.DisplayOrder)
            .Select(z => new LookupItem<int>(z.LeadTicketStatus, z.Id, string.Empty, 0))
            .ToListAsync();

        return lookupItems.ToArray();
    }

    public async Task<LookupItem<int>[]> GetGracePeriodsAsync()
    {
        var lookupItems = await lookupsDb.LookUpGracePeriods.AsNoTracking()
            .OrderBy(z => z.DisplayOrder)
            .Select(z => new LookupItem<int>(z.GracePeriod, z.GracePeriodHours, string.Empty, 0))
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<LookupItem<int>[]> GetCorporateCompaniesAsync()
    {
        var lookupItems = await lookupsDb.LookupCorporateCompanies.AsNoTracking()
            .OrderBy(z => z.CorporateCompany)
            .Select(z => new LookupItem<int>(z.CorporateCompany, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<PrivilegeItem[]> GetSystemPrivilegesAsync()
    {
        var items = await lookupsDb.Privileges.AsNoTracking()
            .ToArrayAsync();

        var lookupItems = items.Select(z => new PrivilegeItem
        {
            ActionName = z.ActionName,
            ControllerName = z.ControllerName,
            Path = z.Path,
            Privilege = (SystemPrivileges)z.Id,
            PrivilegeCategory = (PrivilegeCategories)z.PrivilegeCategoryId,
            PrivilegeMetaData = z.PrivilegeMetaData
        })
            .ToArray();

        return lookupItems;
    }

    public async Task<ContactingTypeItem[]> GetContactingTypesAsync()
    {
        var items = await lookupsDb.LookUpContactingTypes.AsNoTracking()
            .OrderBy(z => z.DisplayOrder)
            .ToArrayAsync();

        var lookupItems = items.Select(z => new ContactingTypeItem
        {
            ContactingTypeName = z.ContactingType,
            ContactingType = (ContactingTypes)z.Id,
            Id = z.Id,
            IsEssential = z.IsEssential,
            IsFeedBack = z.CountAsFeedBack,
            NeedsDate = z.NeedsDate
        })
            .ToArray();

        return lookupItems;
    }

    public async Task<LookupItem<int>[]> GetVoidReasonsAsync()
    {
        var lookupItems = await lookupsDb.LookUpVoidReasons.AsNoTracking()
            .OrderBy(z => z.VoidReason)
            .Select(z => new LookupItem<int>(z.VoidReason, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<LookupItem<int>[]> GetPropertyTypesAsync()
    {
        var lookupItems = await lookupsDb.LookUpPropertyTypes.AsNoTracking()
            .OrderBy(z => z.PropertyType)
            .Select(z => new LookupItem<int>(z.PropertyType, z.Id, string.Empty, z.UsageId))
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<ActiveUserItem[]> GetActiveUsersAsync()
    {
        var lookupItems = await lookupsDb.Users.Where(z => z.IsApproved)
            .AsNoTracking()
            .OrderBy(z => z.FullNameLowered)
            .Select(z => new ActiveUserItem
            {
                UserId = z.Id,
                FullName = z.FullName,
                Email = z.Email,
                UserName = z.UserName,
                FullNameLowered = z.FullNameLowered
            })
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<LookupItem<int>[]> GetOutsideBrokersAsync()
    {
        var lookupItems = await lookupsDb.LookUpOutsideBrokers.AsNoTracking()
            .OrderBy(z => z.OutsideBroker)
            .Select(z => new LookupItem<int>(z.OutsideBroker, z.Id, z.OutsideBrokerCommissionPercentage.ToString("#.#####"), 0))
            .ToArrayAsync();

        return lookupItems;
    }

    public async Task<LookupItem<int>[]> GetDevelopersAsync()
    {
        var rawDevelopers = await lookupsDb.ConstructionDevelopers.Include(z => z.ConstructionDeveloperProjects)
            .AsNoTracking()
            .ToArrayAsync();

        var developers = new LookupItem<int>[rawDevelopers.Length];

        for (var idx = 0; idx < rawDevelopers.Length; idx++)
        {
            var rawDeveloper = rawDevelopers[idx];
            var developer = new LookupItem<int>(rawDeveloper.DeveloperName, rawDeveloper.Id, "", 0);

            var projects = rawDeveloper.ConstructionDeveloperProjects.OrderBy(z => z.ProjectName)
                .Select(z => new LookupItem<int>(z.ProjectName, z.Id, string.Empty, rawDeveloper.Id))
                .ToArray();

            developer.SubLookUps = projects;
            developers.SetValue(developer, idx);
        }

        return developers.OrderBy(z => z.ItemName)
            .ToArray();
    }

    public async Task<LookupItem<int>[]> GetPrimeTcrStatusesAsync()
    {
        var items = await lookupsDb.LookUpPrimeTcrStatuses.AsNoTracking()
            .Select(z => new LookupItem<int>(z.PrimeTcrStatus, z.Id, string.Empty, 0))
            .ToArrayAsync();

        return items;
    }

    public async Task<DashboardNotificationItem[]> GetDashboardNotificationsAsync()
    {
        var rawItems = await lookupsDb.DashboardNotifications.AsNoTracking()

          .ToArrayAsync();

        var items = rawItems.Select(z => new DashboardNotificationItem
        {
            DepartmentId = z.DepartmentId,
            Label = z.Label,
            LinkUrl = z.LinkUrl,
            Order = z.Order,
            PositionId = z.PositionId,
            PropertyName = z.PropertyName,
            SalesAgents = z.SalesAgents,
            SalesMangers = z.SalesMangers,
            SystemEntityType = (EntityTypes)z.SystemEntityId,
        })
            .ToArray();
        return items;
    }

    public async Task<LocationItem[]> GetLocationsAsync()
    {
        var rawItems = await lookupsDb.VwLookupLocations.AsNoTracking()

          .ToArrayAsync();

        var items = rawItems.Select(z => new LocationItem
        {
            CityId = z.CityId,
            CountryId = z.CountryId,
            DistrictId = z.DistrictId,
            NeighborhoodId = z.Id,
            City = z.City,
            Country = z.Country,
            District = z.District,
            Neighborhood = z.Neighborhood
        })
            .ToArray();
        return items;
    }
}