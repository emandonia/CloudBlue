using CloudBlue.Data.Configurations.App;
using CloudBlue.Data.Configurations.Lookups;
using CloudBlue.Data.Configurations.Users;
using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DataModels.CbUsers;
using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DataModels.Tenants;
using CloudBlue.Domain.Interfaces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.DataContext;

public class LookUpsDataContext : DbContext, ILookUpsDataContext
{
    public LookUpsDataContext()
    {
    }

    public LookUpsDataContext(DbContextOptions<LookUpsDataContext> options) : base(options)
    {
    }

    public async Task SaveBulkChangesAsync()
    {
        await this.BulkSaveChangesAsync();
    }

    public DbSet<LookUpGracePeriod> LookUpGracePeriods { get; set; }
    public DbSet<VwLookupLocation> VwLookupLocations { get; set; }
    public DbSet<FranchiseBranch> Branches { get; set; }
    public DbSet<DashboardNotification> DashboardNotifications { get; set; }
    public DbSet<ConstructionDeveloperProject> ConstructionDeveloperProjects { get; set; }
    public DbSet<ConstructionDeveloper> ConstructionDevelopers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<LookUpOutsideBroker> LookUpOutsideBrokers { get; set; }
    public DbSet<Privilege> Privileges { get; set; }
    public DbSet<PrivilegeCategory> PrivilegeCategories { get; set; }
    public DbSet<PrivilegeScope> PrivilegeScopes { get; set; }
    public DbSet<PrivilegeEntityType> PrivilegeEntityTypes { get; set; }
    public DbSet<FranchiseCompany> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<UserPosition> UserPositions { get; set; }
    public DbSet<LookUpAddressType> LookUpAddressTypes { get; set; }
    public DbSet<LookUpBlindLeadTicketStatus> LookUpBlindLeadTicketStatuses { get; set; }
    public DbSet<LookUpCallStatus> LookUpCallStatuses { get; set; }
    public DbSet<LookUpCallType> LookUpCallTypes { get; set; }
    public DbSet<LookUpCity> LookUpCities { get; set; }
    public DbSet<LookupCorporateCompany> LookupCorporateCompanies { get; set; }
    public DbSet<LookUpClientCategory> LookUpClientCategories { get; set; }
    public DbSet<LookUpClientStatus> LookUpClientStatuses { get; set; }
    public DbSet<LookUpClientTitle> LookUpClientTitles { get; set; }
    public DbSet<LookUpClientType> LookUpClientTypes { get; set; }
    public DbSet<LookUpContactingType> LookUpContactingTypes { get; set; }
    public DbSet<LookUpConversionRate> LookUpConversionRates { get; set; }
    public DbSet<LookUpCountry> LookUpCountries { get; set; }
    public DbSet<LookUpCurrency> LookUpCurrencies { get; set; }
    public DbSet<LookUpDealType> LookUpDealTypes { get; set; }
    public DbSet<LookUpDeviceType> LookUpDeviceTypes { get; set; }
    public DbSet<LookUpDistrict> LookUpDistricts { get; set; }
    public DbSet<LookUpEntityType> LookUpEntityTypes { get; set; }
    public DbSet<LookUpEventProcess> LookUpEventProcesses { get; set; }
    public DbSet<LookUpEventType> LookUpEventTypes { get; set; }
    public DbSet<LookUpGender> LookUpGenders { get; set; }
    public DbSet<LookUpKnowSource> LookUpKnowSources { get; set; }
    public DbSet<LookUpKnowSubSource> LookUpKnowSubSources { get; set; }
    public DbSet<LookUpLeadSource> LookUpLeadSources { get; set; }
    public DbSet<LookUpLeadTicketStatus> LookUpLeadTicketStatuses { get; set; }
    public DbSet<LookUpNeighborhood> LookUpNeighborhoods { get; set; }
    public DbSet<LookUpOccupationField> LookUpOccupationFields { get; set; }
    public DbSet<LookUpPrimeTcrStatus> LookUpPrimeTcrStatuses { get; set; }
    public DbSet<LookUpPropertyType> LookUpPropertyTypes { get; set; }
    public DbSet<LookUpResaleTcrStatus> LookUpResaleTcrStatuses { get; set; }
    public DbSet<LookUpRotationStatus> LookUpRotationStatuses { get; set; }
    public DbSet<LookUpSalesAccountingFeedBack> LookUpSalesAccountingFeedBacks { get; set; }
    public DbSet<LookUpSalesType> LookUpSalesTypes { get; set; }
    public DbSet<LookUpService> LookUpServices { get; set; }
    public DbSet<LookUpUsage> LookUpUsages { get; set; }
    public DbSet<LookUpVoidReason> LookUpVoidReasons { get; set; }
    public DbSet<LookUpWorkField> LookUpWorkFields { get; set; }
    public DbSet<MarketingAgency> MarketingAgencies { get; set; }
    public DbSet<UsageToEntity> UsageToEntities { get; set; }
    public DbSet<VwAgent> VwAgents { get; set; }
    public DbSet<VwCallRecipient> VwCallRecipients { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VwAgentConfiguration());
        modelBuilder.ApplyConfiguration(new VwCallRecipientConfiguration());
        modelBuilder.ApplyConfiguration(new LookUpCityConfiguration());
        modelBuilder.ApplyConfiguration(new LookUpDistrictConfiguration());
        modelBuilder.ApplyConfiguration(new LookUpKnowSubSourceConfiguration());
        modelBuilder.ApplyConfiguration(new LookUpNeighborhoodConfiguration());
        modelBuilder.ApplyConfiguration(new PrivilegeConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ConstructionDeveloperConfiguration());
        modelBuilder.ApplyConfiguration(new DashboardNotificationConfiguration());
        modelBuilder.ApplyConfiguration(new VwLookupLocationConfiguration());
    }
}