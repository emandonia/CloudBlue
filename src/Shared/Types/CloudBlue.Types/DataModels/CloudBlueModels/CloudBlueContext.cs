using Microsoft.EntityFrameworkCore;

namespace DataImporterLib.CloudBlueModels;

public partial class CloudBlueContext : DbContext
{
    public CloudBlueContext()
    {
    }

    public CloudBlueContext(DbContextOptions<CloudBlueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAddress> ClientAddresses { get; set; }

    public virtual DbSet<ClientContactDevice> ClientContactDevices { get; set; }

    public virtual DbSet<CustomizedAction> CustomizedActions { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<EntityPrivilege> EntityPrivileges { get; set; }

    public virtual DbSet<FillingOption> FillingOptions { get; set; }

    public virtual DbSet<FranchiseBranch> FranchiseBranches { get; set; }

    public virtual DbSet<FranchiseCompany> FranchiseCompanies { get; set; }

    public virtual DbSet<HistoryStore> HistoryStores { get; set; }

    public virtual DbSet<LeadTicket> LeadTickets { get; set; }

    public virtual DbSet<LeadTicketExtra> LeadTicketExtras { get; set; }

    public virtual DbSet<LookUpAddressType> LookUpAddressTypes { get; set; }

    public virtual DbSet<LookUpCallStatus> LookUpCallStatuses { get; set; }

    public virtual DbSet<LookUpCallType> LookUpCallTypes { get; set; }

    public virtual DbSet<LookUpCity> LookUpCities { get; set; }

    public virtual DbSet<LookUpClientCategory> LookUpClientCategories { get; set; }

    public virtual DbSet<LookUpClientTitle> LookUpClientTitles { get; set; }

    public virtual DbSet<LookUpClientType> LookUpClientTypes { get; set; }

    public virtual DbSet<LookUpContactStatus> LookUpContactStatuses { get; set; }

    public virtual DbSet<LookUpContactType> LookUpContactTypes { get; set; }

    public virtual DbSet<LookUpContactingType> LookUpContactingTypes { get; set; }

    public virtual DbSet<LookUpConversionRate> LookUpConversionRates { get; set; }

    public virtual DbSet<LookUpCountry> LookUpCountries { get; set; }

    public virtual DbSet<LookUpCurrency> LookUpCurrencies { get; set; }

    public virtual DbSet<LookUpDealType> LookUpDealTypes { get; set; }

    public virtual DbSet<LookUpDeviceType> LookUpDeviceTypes { get; set; }

    public virtual DbSet<LookUpDistrict> LookUpDistricts { get; set; }

    public virtual DbSet<LookUpEntityType> LookUpEntityTypes { get; set; }

    public virtual DbSet<LookUpEventProcess> LookUpEventProcesses { get; set; }

    public virtual DbSet<LookUpEventType> LookUpEventTypes { get; set; }

    public virtual DbSet<LookUpKnowSource> LookUpKnowSources { get; set; }

    public virtual DbSet<LookUpKnowSubSource> LookUpKnowSubSources { get; set; }

    public virtual DbSet<LookUpLeadSource> LookUpLeadSources { get; set; }

    public virtual DbSet<LookUpLeadTicketStatus> LookUpLeadTicketStatuses { get; set; }

    public virtual DbSet<LookUpNeighborhood> LookUpNeighborhoods { get; set; }

    public virtual DbSet<LookUpPrimeTcrstatus> LookUpPrimeTcrStatuses { get; set; }

    public virtual DbSet<LookUpPropertyType> LookUpPropertyTypes { get; set; }

    public virtual DbSet<LookUpResaleTcrstatus> LookUpResaleTcrStatuses { get; set; }

    public virtual DbSet<LookUpRotationStatus> LookUpRotationStatuses { get; set; }

    public virtual DbSet<LookUpSalesAccountingFeedBack> LookUpSalesAccountingFeedBacks { get; set; }

    public virtual DbSet<LookUpSalesType> LookUpSalesTypes { get; set; }

    public virtual DbSet<LookUpService> LookUpServices { get; set; }

    public virtual DbSet<LookUpUsage> LookUpUsages { get; set; }

    public virtual DbSet<LookUpVoidReason> LookUpVoidReasons { get; set; }

    public virtual DbSet<LookUpWorkField> LookUpWorkFields { get; set; }

    public virtual DbSet<PrimeTcrstatus> PrimeTcrStatuses { get; set; }

    public virtual DbSet<Privilege> Privileges { get; set; }

    public virtual DbSet<PrivilegeCategory> PrivilegeCategories { get; set; }

    public virtual DbSet<PrivilegeScope> PrivilegeScopes { get; set; }

    public virtual DbSet<SalesPromotion> SalesPromotions { get; set; }

    public virtual DbSet<SalesUserTree> SalesUserTrees { get; set; }

    public virtual DbSet<UsageToEntity> UsageToEntities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserPhone> UserPhones { get; set; }

    public virtual DbSet<UserPosition> UserPositions { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=64.150.184.135;port=5543; Database=CloudBlue;uid=postgres;pwd=CbDev@c6!2024);");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Call>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Calls_PK_Call");

            entity.HasIndex(e => e.BranchId, "Calls_BranchIdx");

            entity.HasIndex(e => e.CallTypeId, "Calls_CalltypeIdx");

            entity.HasIndex(e => e.CompanyId, "Calls_CompanyFKIdx");

            entity.HasIndex(e => e.ClientId, "Calls_DefSubcontactIdx");

            entity.HasIndex(e => e.CreationDateNumeric, "Calls_creationdateIdx");

            entity.HasIndex(e => e.CallStatusId, "Calls_statusIdx");

            entity.Property(e => e.CallNote).HasMaxLength(2500);
            entity.Property(e => e.CallTypeOther).HasMaxLength(150);
            entity.Property(e => e.CanceledDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.HandledDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.KnowDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.KnowSourceExtra).HasMaxLength(250);
            entity.Property(e => e.SourceExtra).HasMaxLength(250);
            entity.Property(e => e.StatusReason).HasColumnType("character varying");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Clients_PK__Contact__5C66CEEBE587CD38");

            entity.Property(e => e.ClientCompanyName).HasMaxLength(150);
            entity.Property(e => e.ClientName).HasMaxLength(150);
            entity.Property(e => e.ClientNameAr)
                .HasMaxLength(150)
                .HasColumnName("ClientNameAR");
            entity.Property(e => e.ClientOccupation).HasColumnType("character varying");
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Notes).HasMaxLength(2500);
        });

        modelBuilder.Entity<ClientAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClientAddresses_PK_Address");

            entity.HasIndex(e => e.ClientId, "IX_ClientAddresses_ClientId").IsUnique();

            entity.Property(e => e.AddressAr)
                .HasMaxLength(500)
                .HasColumnName("AddressAR");
            entity.Property(e => e.AddressInfo).HasMaxLength(250);

            entity.HasOne(d => d.Client).WithOne(p => p.ClientAddress).HasForeignKey<ClientAddress>(d => d.ClientId);
        });

        modelBuilder.Entity<ClientContactDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ClientContactDevices_PK_ClientContactDevices");

            entity.HasIndex(e => e.ClientId, "IX_ClientContactDevices_ClientId");

            entity.Property(e => e.DeviceInfo).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasColumnType("character varying");
            entity.Property(e => e.PhoneAreaCode).HasColumnType("character varying");
            entity.Property(e => e.PhoneCountryCode).HasColumnType("character varying");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientContactDevices).HasForeignKey(d => d.ClientId);
        });

        modelBuilder.Entity<CustomizedAction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CustomizedAction");

            entity.Property(e => e.CustomizedActionName).HasMaxLength(250);
            entity.Property(e => e.CustomizedActionPk)
                .ValueGeneratedOnAdd()
                .HasColumnName("CustomizedActionPK");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Departments_PK_Department");

            entity.Property(e => e.DepartmentName).HasMaxLength(150);
        });

        modelBuilder.Entity<EntityPrivilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EntityPrivileges_PK_userrightToGroup");

            entity.HasIndex(e => e.PrivilegeScopeId, "IX_EntityPrivileges_PrivilegeScopeId");

            entity.HasOne(d => d.PrivilegeScope).WithMany(p => p.EntityPrivileges)
                .HasForeignKey(d => d.PrivilegeScopeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EntityPri__Privi__1209AD79");
        });

        modelBuilder.Entity<FillingOption>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("FillingOption");

            entity.Property(e => e.AddressType).HasMaxLength(50);
            entity.Property(e => e.AgeItem).HasMaxLength(50);
            entity.Property(e => e.AgeType).HasMaxLength(50);
            entity.Property(e => e.AgreementType).HasMaxLength(50);
            entity.Property(e => e.Amenities).HasMaxLength(50);
            entity.Property(e => e.ApplianceItem).HasMaxLength(50);
            entity.Property(e => e.AreaType).HasMaxLength(50);
            entity.Property(e => e.AreaUnit).HasMaxLength(50);
            entity.Property(e => e.CallStatus).HasMaxLength(50);
            entity.Property(e => e.CallType).HasMaxLength(50);
            entity.Property(e => e.CeilingHightType).HasMaxLength(150);
            entity.Property(e => e.CeilingType).HasMaxLength(150);
            entity.Property(e => e.CommercialLicenseItem).HasMaxLength(150);
            entity.Property(e => e.ContactStatus).HasMaxLength(50);
            entity.Property(e => e.ContactType).HasMaxLength(50);
            entity.Property(e => e.ContactingType).HasMaxLength(50);
            entity.Property(e => e.DeviceType).HasMaxLength(50);
            entity.Property(e => e.DimensionUnit).HasMaxLength(50);
            entity.Property(e => e.DoorsFeature).HasMaxLength(50);
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.EventProcess).HasMaxLength(50);
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.ExteriorFinishing).HasMaxLength(50);
            entity.Property(e => e.FacilityItem).HasMaxLength(50);
            entity.Property(e => e.FacilityOtherItem).HasMaxLength(50);
            entity.Property(e => e.FinishingFloorTypeItem).HasMaxLength(50);
            entity.Property(e => e.FloorLevel).HasMaxLength(150);
            entity.Property(e => e.FrontageItem).HasMaxLength(50);
            entity.Property(e => e.InteriorFinishing).HasMaxLength(50);
            entity.Property(e => e.KitchenType).HasMaxLength(150);
            entity.Property(e => e.KnowSource).HasMaxLength(50);
            entity.Property(e => e.LayoutItem).HasMaxLength(50);
            entity.Property(e => e.LeadRatingBudgetRange).HasMaxLength(150);
            entity.Property(e => e.LeadRatingSeriousness).HasMaxLength(150);
            entity.Property(e => e.LeadTicketStatus).HasMaxLength(50);
            entity.Property(e => e.LicenseItem).HasMaxLength(50);
            entity.Property(e => e.Localization)
                .HasMaxLength(50)
                .HasColumnName("localization");
            entity.Property(e => e.OldLeadTicketStatus).HasMaxLength(50);
            entity.Property(e => e.PaymentFrequency).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PermissionRequestStatus).HasMaxLength(150);
            entity.Property(e => e.PricingType).HasMaxLength(50);
            entity.Property(e => e.PropertyMarketingPupose).HasMaxLength(50);
            entity.Property(e => e.PropertyMarketingType).HasMaxLength(50);
            entity.Property(e => e.PropertyProspectStatus).HasMaxLength(50);
            entity.Property(e => e.PropertyRentOption).HasMaxLength(50);
            entity.Property(e => e.RecId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RecID");
            entity.Property(e => e.ReferralStatus).HasMaxLength(50);
            entity.Property(e => e.RequestStatus).HasMaxLength(50);
            entity.Property(e => e.ResalePaymentType).HasMaxLength(150);
            entity.Property(e => e.SalesType).HasMaxLength(50);
            entity.Property(e => e.SellingPoint).HasMaxLength(150);
            entity.Property(e => e.Source).HasMaxLength(50);
            entity.Property(e => e.StructureType).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UnitStatus).HasMaxLength(50);
            entity.Property(e => e.ViewItem).HasMaxLength(50);
            entity.Property(e => e.WindowsFeature).HasMaxLength(50);
        });

        modelBuilder.Entity<FranchiseBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FranchiseBranches_PK_BranchPK");

            entity.HasIndex(e => e.CompanyId, "FranchiseBranches_companyIdx");

            entity.Property(e => e.BranchName).HasMaxLength(250);
            entity.Property(e => e.Disabled).HasDefaultValue(false);
            entity.Property(e => e.FillColor).HasMaxLength(255);
            entity.Property(e => e.PropertyAutoAproval).HasDefaultValue(false);
            entity.Property(e => e.RequestAutoAproval).HasDefaultValue(false);
            entity.Property(e => e.TextColor).HasMaxLength(255);

            entity.HasOne(d => d.Company).WithMany(p => p.FranchiseBranches)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__Franchise__Compa__55009F39");
        });

        modelBuilder.Entity<FranchiseCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FranchiseCompanies_PK_CompanyFranchisee");

            entity.Property(e => e.CompanyName).HasMaxLength(300);
        });

        modelBuilder.Entity<HistoryStore>(entity =>
        {
            entity.HasKey(e => new { e.TableName, e.PkDateDest }).HasName("primary");

            entity.ToTable("history_store");

            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("table_name");
            entity.Property(e => e.PkDateDest)
                .HasMaxLength(400)
                .HasColumnName("pk_date_dest");
            entity.Property(e => e.PkDateSrc)
                .HasMaxLength(400)
                .HasColumnName("pk_date_src");
            entity.Property(e => e.RecordState).HasColumnName("record_state");
            entity.Property(e => e.Timemark)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timemark");
        });

        modelBuilder.Entity<LeadTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LeadTickets_PK_LeadTicket");

            entity.HasIndex(e => e.CompanyId, "LeadTickets_CompanyIdIdx");

            entity.HasIndex(e => e.ClientId, "LeadTickets_ContactfkIdx");

            entity.HasIndex(e => e.ConvertedFromDummy, "LeadTickets_ConvertedFromDummyIdx");

            entity.HasIndex(e => e.CreationDateNumeric, "LeadTickets_CreationDateLGIdx");

            entity.HasIndex(e => e.IsFullLeadTicket, "LeadTickets_IsFullLeadsIdx");

            entity.HasIndex(e => e.BranchId, "LeadTickets_LeadTicketBranchIdx");

            entity.HasIndex(e => e.CurrentAgentId, "LeadTickets_LeadTicketCurrentAgentIdx");

            entity.HasIndex(e => e.LeadTicketStatusId, "LeadTickets_LeadTicketStatusIdx");

            entity.HasIndex(e => e.LineofBusinessId, "LeadTickets_LineOfBusinessLeadsIdx");

            entity.HasIndex(e => e.DistrictId, "LeadTickets_NonClusteredIndex-20160121-170618");

            entity.HasIndex(e => e.NeighborhoodId, "LeadTickets_NonClusteredIndex-20160121-171220");

            entity.HasIndex(e => e.CallId, "LeadTickets_callfkIdx");

            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.KnowDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.KnowSourceExtra).HasMaxLength(250);
            entity.Property(e => e.LeadTicketNote).HasMaxLength(2500);
            entity.Property(e => e.LeadTicketPaperSerial).HasMaxLength(50);
            entity.Property(e => e.OtherNeighbouhood).HasMaxLength(100);
            entity.Property(e => e.ServiceOther).HasMaxLength(150);
            entity.Property(e => e.SourceExtra).HasMaxLength(250);

            entity.HasOne(d => d.Client).WithMany(p => p.LeadTickets)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeadTickets_Clients");
        });

        modelBuilder.Entity<LeadTicketExtra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LeadTicketExtras_PK_LeadTicketExtra");

            entity.HasIndex(e => e.AgentLastEventCreationDateTimeNumeric, "LeadTicketExtras_AgentLastEventCreationDateTimeNumeric");

            entity.HasIndex(e => e.AgentLastEventId, "LeadTicketExtras_AgentLastEventId");

            entity.HasIndex(e => e.CampaignOwnerId, "LeadTicketExtras_CampaignOwnerId");

            entity.HasIndex(e => e.CampaignOwnerId, "LeadTicketExtras_CampaignOwnerIdx");

            entity.HasIndex(e => e.ContactingDeviceId, "LeadTicketExtras_ContactingDeviceId");

            entity.HasIndex(e => e.EventDateTimeNumeric, "LeadTicketExtras_EventDateTimeNumeric");

            entity.HasIndex(e => e.EventTypeId, "LeadTicketExtras_EventTypeId");

            entity.HasIndex(e => e.LastAssignedDateNumeric, "LeadTicketExtras_LastAssignedDateIdx");

            entity.HasIndex(e => e.LastEventEventProcessId, "LeadTicketExtras_LastEventEventProcessId");

            entity.HasIndex(e => e.SetInProgressDateNumeric, "LeadTicketExtras_SetInProgressDateNum");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AgencyAbbrev).HasMaxLength(25);
            entity.Property(e => e.ApplyTwentyFourHoursRules).HasDefaultValue(false);
            entity.Property(e => e.CallBackDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CurrentAgentManagersTree).HasMaxLength(1200);
            entity.Property(e => e.EventComment).HasMaxLength(1200);
            entity.Property(e => e.EventDateTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ExtendedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsPureOld).HasColumnName("isPureOld");
            entity.Property(e => e.LastAssignedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastDeactivatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastRevivalDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastVoidedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LasteId).HasColumnName("lasteId");
            entity.Property(e => e.RejectDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.RejectReason).HasMaxLength(1200);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.LeadTicketExtra)
                .HasForeignKey<LeadTicketExtra>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LeadTicketExtras_LeadTickets");
        });

        modelBuilder.Entity<LookUpAddressType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpAddressTypes_PK__adrs__360414FF792FCC38");

            entity.Property(e => e.AddressType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpCallStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpCallStatuses_PK__cs__360414FF792FCC38");

            entity.Property(e => e.CallStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpCallType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpCallTypes_PK__clst__360414FF792FCC38");

            entity.Property(e => e.CallType).HasMaxLength(50);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
        });

        modelBuilder.Entity<LookUpCity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpCities_PK_City");

            entity.HasIndex(e => e.CountryId, "LookUpCities_countryIdx");

            entity.Property(e => e.CityName).HasMaxLength(150);

            entity.HasOne(d => d.Country).WithMany(p => p.LookUpCities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LookUpCit__Count__0F624AF8");
        });

        modelBuilder.Entity<LookUpClientCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpClientCategories_PK__ClientCa__BDD52168FD5C646C");

            entity.Property(e => e.ClientCategory).HasMaxLength(255);
        });

        modelBuilder.Entity<LookUpClientTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpClientTitles_PK__titl__360414FF792FCC38");

            entity.Property(e => e.ClientTitle).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpClientType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpClientTypes_PK__ClientType__360414FF792FCC38");

            entity.Property(e => e.ClientType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpContactStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpContactStatuses_PK__cst__360414FF792FCC38");

            entity.Property(e => e.ContactStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpContactType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpContactTypes_PK__ContactType__360414FF792FCC38");

            entity.Property(e => e.ContactType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpContactingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpContactingTypes_PK__cvfr__360414FF792FCC38");

            entity.Property(e => e.ContactingType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpConversionRate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpConversionRates_PK_ConversionRate");
        });

        modelBuilder.Entity<LookUpCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpCountries_PK_Country");

            entity.Property(e => e.CountryName).HasMaxLength(50);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(1000);
            entity.Property(e => e.PhoneCode).HasMaxLength(20);
        });

        modelBuilder.Entity<LookUpCurrency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpCurrencies_PK__LookUpCu__3214EC0708469A8E");

            entity.Property(e => e.CurrencyName).HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(6);
        });

        modelBuilder.Entity<LookUpDealType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpDealTypes_PK__DealType__3214EC07AF2011EE");

            entity.Property(e => e.DealTypeName).HasMaxLength(255);
        });

        modelBuilder.Entity<LookUpDeviceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpDeviceTypes_PK__DT__360414FF792FCC38");

            entity.Property(e => e.DeviceType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpDistrict>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpDistricts_PK_District");

            entity.HasIndex(e => e.CityId, "LookUpDistricts_CityIdx");

            entity.Property(e => e.DistrictName).HasMaxLength(250);
            entity.Property(e => e.DistrictNameAra).HasMaxLength(250);

            entity.HasOne(d => d.City).WithMany(p => p.LookUpDistricts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LookUpDis__CityI__114A936A");
        });

        modelBuilder.Entity<LookUpEntityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpEntityTypes_PK__enty__360414FF792FCC38");

            entity.Property(e => e.EntityType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpEventProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpEventProcesses_PK_evst___360414FF792FCC38");

            entity.Property(e => e.EventProcess).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpEventType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpEventTypes_PK__evt__360414FF792FCC38");

            entity.Property(e => e.EventTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpKnowSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpKnowSources_PK__ks__360414FF792FCC38");

            entity.Property(e => e.KnowSourceName).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpKnowSubSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpKnowSubSources_PK_SourceItem");

            entity.HasIndex(e => e.KnowSourceId, "LookUpKnowSubSources_KnowSourceIdx");

            entity.Property(e => e.Abbrev).HasMaxLength(255);
            entity.Property(e => e.KnowSubSource).HasMaxLength(255);

            entity.HasOne(d => d.KnowSource).WithMany(p => p.LookUpKnowSubSources)
                .HasForeignKey(d => d.KnowSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LookUpKno__KnowS__1BC821DD");
        });

        modelBuilder.Entity<LookUpLeadSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpLeadSources_PK__ldsr__360414FF792FCC38");

            entity.Property(e => e.LeadSourceName).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpLeadTicketStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpLeadTicketStatuses_PK__ldst__360414FF792FCC38");

            entity.Property(e => e.LeadTicketStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpNeighborhood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpNeighborhoods_PK_Neighborhood");

            entity.HasIndex(e => e.DistrictId, "LookUpNeighborhoods_DistrictIdx");

            entity.Property(e => e.NeighborhoodName).HasMaxLength(250);
            entity.Property(e => e.NeighborhoodNameAra).HasMaxLength(250);
            entity.Property(e => e.TypeId).HasColumnName("typeId");
            entity.Property(e => e.WebSiteName).HasColumnType("character varying");

            entity.HasOne(d => d.District).WithMany(p => p.LookUpNeighborhoods)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LookUpNei__Distr__1332DBDC");
        });

        modelBuilder.Entity<LookUpPrimeTcrstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpPrimeTCRStatuses_PK__PrimeTCR__DC18E61C28A2FFC9");

            entity.ToTable("LookUpPrimeTCRStatuses");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PrimeTcrstatusName)
                .HasMaxLength(50)
                .HasColumnName("PrimeTCRStatusName");
        });

        modelBuilder.Entity<LookUpPropertyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpPropertyTypes_PK__LookUpPr__3214EC07BC13E624");

            entity.Property(e => e.Pfabbrev)
                .HasMaxLength(255)
                .HasColumnName("PFAbbrev");
            entity.Property(e => e.PropertyType).HasMaxLength(150);
            entity.Property(e => e.PropertyTypeArabic).HasMaxLength(150);
        });

        modelBuilder.Entity<LookUpResaleTcrstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpResaleTCRStatuses_PK__ResaleTC__3214EC07B01607B5");

            entity.ToTable("LookUpResaleTCRStatuses");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ResaleTcrstatusName)
                .HasMaxLength(50)
                .HasColumnName("ResaleTCRStatusName");
        });

        modelBuilder.Entity<LookUpRotationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpRotationStatuses_PK__Rotation__3213E83F1E4BDEA4");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.IsRunning).HasColumnName("isRunning");
        });

        modelBuilder.Entity<LookUpSalesAccountingFeedBack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpSalesAccountingFeedBacks_PK__SalesAcc__3214EC074C80E0BE");

            entity.Property(e => e.SalesAccountingFeedBackName).HasMaxLength(150);
        });

        modelBuilder.Entity<LookUpSalesType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpSalesTypes_PK__SalesType__360414FF792FCC38");

            entity.Property(e => e.SalesType).HasMaxLength(50);
        });

        modelBuilder.Entity<LookUpService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpServices_PK__Service__3214EC0704DF9524");

            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Service).HasMaxLength(100);
        });

        modelBuilder.Entity<LookUpUsage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpUsages_PK_LineOfBusiness");

            entity.Property(e => e.Usage).HasMaxLength(150);
        });

        modelBuilder.Entity<LookUpVoidReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpVoidReasons_PK__VoidReas__3214EC073CF50C1F");

            entity.Property(e => e.VoidReason).HasMaxLength(255);
        });

        modelBuilder.Entity<LookUpWorkField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LookUpWorkFields_PK__LookUpWorkField__360414FF792FCC38");

            entity.Property(e => e.WorkField).HasMaxLength(50);
        });

        modelBuilder.Entity<PrimeTcrstatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PrimeTCRStatus");

            entity.Property(e => e.PrimeTcrstatusName)
                .HasMaxLength(50)
                .HasColumnName("PrimeTCRStatusName");
            entity.Property(e => e.PrimeTcrstatusPk).HasColumnName("PrimeTCRStatusPK");
        });

        modelBuilder.Entity<Privilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Privileges_PK_userright");

            entity.HasIndex(e => e.PrivilegeCategoryId, "IX_Privileges_PrivilegeCategoryId");

            entity.Property(e => e.ActionName).HasMaxLength(255);
            entity.Property(e => e.ControllerName).HasMaxLength(255);
            entity.Property(e => e.PrivilegeMetaData).HasMaxLength(255);
            entity.Property(e => e.PrivilegeName).HasMaxLength(1200);

            entity.HasOne(d => d.PrivilegeCategory).WithMany(p => p.Privileges)
                .HasForeignKey(d => d.PrivilegeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Privilege__Privi__0697FACD");
        });

        modelBuilder.Entity<PrivilegeCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrivilegeCategories_PK__Privileg__3214EC079D9D5728");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PrivilegeCategoryName).HasMaxLength(120);
        });

        modelBuilder.Entity<PrivilegeScope>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PrivilegeScopes_PK__Privileg__3214EC0709C309AF");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PrivilegeScopeName).HasMaxLength(255);
        });

        modelBuilder.Entity<SalesPromotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SalesPromotions_PK__SalesPro__7EE01F91EF461031");

            entity.Property(e => e.Created).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PromotionEndDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PromotionStartDate).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<SalesUserTree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SalesUserTrees_PK__SalesUse__13C68212FACA39EB");

            entity.Property(e => e.Created).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserFullName).HasColumnType("character varying");
        });

        modelBuilder.Entity<UsageToEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UsageToEntity_PK_LineOfBusinessToEntity");

            entity.ToTable("UsageToEntity");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CreateDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DirectManagerId).HasDefaultValueSql("'0'::bigint");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.HireDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LastLockoutDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastLoginDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastPasswordChangedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastPasswordFailureDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastPromotionDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.LastUpdateDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Password).HasMaxLength(128);
            entity.Property(e => e.PasswordSalt).HasMaxLength(128);
            entity.Property(e => e.ResetPasswordKey).HasMaxLength(128);
            entity.Property(e => e.ResignDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.SalesForceUserId).HasMaxLength(120);
            entity.Property(e => e.TopMostManagerName).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserGroups_PK_usergroup");

            entity.Property(e => e.UserGroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserPhone>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Created).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeviceInfo).HasMaxLength(150);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Phone).HasMaxLength(16);
            entity.Property(e => e.PhoneAreaCode).HasMaxLength(4);
            entity.Property(e => e.PhoneCountryCode).HasMaxLength(6);
        });

        modelBuilder.Entity<UserPosition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserPositions_PK__UserPosi__4EA0768E1C59C2FE");

            entity.HasIndex(e => e.DepartmentId, "IX_UserPositions_DepartmentId");

            entity.HasIndex(e => e.UserGroupId, "IX_UserPositions_UserGroupId");

            entity.Property(e => e.ApplyTargets).HasDefaultValue(false);
            entity.Property(e => e.CanHaveTeam).HasDefaultValue(false);
            entity.Property(e => e.DepartmentId).HasDefaultValue(0);
            entity.Property(e => e.HasLineOfBusiness).HasDefaultValue(false);
            entity.Property(e => e.IsManager).HasDefaultValue(false);
            entity.Property(e => e.PositionOrder).HasDefaultValue(0);
            entity.Property(e => e.UserGroupId).HasDefaultValue(0);
            entity.Property(e => e.UserPositionName).HasMaxLength(255);
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ApiKey).HasMaxLength(120);
            entity.Property(e => e.CreationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeviceServiceId).HasMaxLength(200);
            entity.Property(e => e.ExpireDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.LoginProvider).HasMaxLength(120);
            entity.Property(e => e.SerializedObject).HasMaxLength(4000);
            entity.Property(e => e.SetExpiredOn).HasColumnType("timestamp without time zone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
