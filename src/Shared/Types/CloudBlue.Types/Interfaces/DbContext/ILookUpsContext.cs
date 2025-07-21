using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DataModels.Tenants;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Domain.Interfaces.DbContext;

public interface ILookUpsDataContext
{
    DbSet<LookUpGracePeriod> LookUpGracePeriods { get; set; }
    DbSet<VwLookupLocation> VwLookupLocations { get; set; }
    DbSet<FranchiseBranch> Branches { get; set; }
    DbSet<DashboardNotification> DashboardNotifications { get; set; }
    DbSet<ConstructionDeveloperProject> ConstructionDeveloperProjects { get; set; }
    DbSet<ConstructionDeveloper> ConstructionDevelopers { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<LookUpOutsideBroker> LookUpOutsideBrokers { get; set; }
    DbSet<Privilege> Privileges { get; set; }
    DbSet<PrivilegeCategory> PrivilegeCategories { get; set; }
    DbSet<PrivilegeScope> PrivilegeScopes { get; set; }
    DbSet<PrivilegeEntityType> PrivilegeEntityTypes { get; set; }

    DbSet<FranchiseCompany> Companies { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<UserPosition> UserPositions { get; set; }

    DbSet<LookUpAddressType> LookUpAddressTypes { get; set; }
    DbSet<LookUpBlindLeadTicketStatus> LookUpBlindLeadTicketStatuses { get; set; }
    DbSet<LookUpCallStatus> LookUpCallStatuses { get; set; }
    DbSet<LookUpCallType> LookUpCallTypes { get; set; }
    DbSet<LookUpCity> LookUpCities { get; set; }
    DbSet<LookupCorporateCompany> LookupCorporateCompanies { get; set; }
    DbSet<LookUpClientCategory> LookUpClientCategories { get; set; }
    DbSet<LookUpClientStatus> LookUpClientStatuses { get; set; }
    DbSet<LookUpClientTitle> LookUpClientTitles { get; set; }
    DbSet<LookUpClientType> LookUpClientTypes { get; set; }
    DbSet<LookUpContactingType> LookUpContactingTypes { get; set; }
    DbSet<LookUpConversionRate> LookUpConversionRates { get; set; }
    DbSet<LookUpCountry> LookUpCountries { get; set; }
    DbSet<LookUpCurrency> LookUpCurrencies { get; set; }
    DbSet<LookUpDealType> LookUpDealTypes { get; set; }
    DbSet<LookUpDeviceType> LookUpDeviceTypes { get; set; }
    DbSet<LookUpDistrict> LookUpDistricts { get; set; }
    DbSet<LookUpEntityType> LookUpEntityTypes { get; set; }
    DbSet<LookUpEventProcess> LookUpEventProcesses { get; set; }
    DbSet<LookUpEventType> LookUpEventTypes { get; set; }
    DbSet<LookUpGender> LookUpGenders { get; set; }
    DbSet<LookUpKnowSource> LookUpKnowSources { get; set; }
    DbSet<LookUpKnowSubSource> LookUpKnowSubSources { get; set; }
    DbSet<LookUpLeadSource> LookUpLeadSources { get; set; }
    DbSet<LookUpLeadTicketStatus> LookUpLeadTicketStatuses { get; set; }
    DbSet<LookUpNeighborhood> LookUpNeighborhoods { get; set; }
    DbSet<LookUpOccupationField> LookUpOccupationFields { get; set; }
    DbSet<LookUpPrimeTcrStatus> LookUpPrimeTcrStatuses { get; set; }
    DbSet<LookUpPropertyType> LookUpPropertyTypes { get; set; }
    DbSet<LookUpResaleTcrStatus> LookUpResaleTcrStatuses { get; set; }
    DbSet<LookUpRotationStatus> LookUpRotationStatuses { get; set; }
    DbSet<LookUpSalesAccountingFeedBack> LookUpSalesAccountingFeedBacks { get; set; }
    DbSet<LookUpSalesType> LookUpSalesTypes { get; set; }
    DbSet<LookUpService> LookUpServices { get; set; }
    DbSet<LookUpUsage> LookUpUsages { get; set; }
    DbSet<LookUpVoidReason> LookUpVoidReasons { get; set; }
    DbSet<LookUpWorkField> LookUpWorkFields { get; set; }
    DbSet<MarketingAgency> MarketingAgencies { get; set; }
    DbSet<UsageToEntity> UsageToEntities { get; set; }
    DbSet<VwAgent> VwAgents { get; set; }
    DbSet<VwCallRecipient> VwCallRecipients { get; set; }

    int SaveChanges();
    Task<int> SaveChangesAsync();

    Task SaveBulkChangesAsync();
}