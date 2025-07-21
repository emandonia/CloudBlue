using CloudBlue.BusinessServices;
using CloudBlue.BusinessServices.App;
using CloudBlue.BusinessServices.Crm;
using CloudBlue.BusinessServices.PrimeTcrs;
using CloudBlue.BusinessServices.UsersAccounts;
using CloudBlue.Data.DataContext;
using CloudBlue.Data.Repositories;
using CloudBlue.Data.Repositories.App;
using CloudBlue.Data.Repositories.Crm;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace CloudBlue.Web.Helpers;

public class ServicesBuilder
{
    public void BuildServices(WebApplicationBuilder builder)
    {
        InjectDbContexts(builder);
        InjectRepositories(builder);
        InjectBusinessServices(builder);
    }

    private void InjectBusinessServices(WebApplicationBuilder builder)
    {
        //  builder.Services.AddSingleton<ILoggerService, LoggerService>();
        //builder.Services.AddTransient<IService, BusinessService>();
        //builder.Services.AddTransient<IService>(provider =>
        //{
        //    var businessService = provider.GetRequiredService<BusinessService>();
        //    var logger = provider.GetRequiredService<ILoggerService>();
        //    return new LoggingServiceDecorator(businessService, logger);
        //});
        builder.Services.AddTransient<IApiKeyService, ApiKeyService>();
        builder.Services.AddTransient<IDevelopersService, DevelopersService>();
        builder.Services.AddTransient<ILookUpsManager, LookUpsManager>();
        builder.Services.AddTransient<IOutsideBrokersService, OutsideBrokersService>();
        builder.Services.AddTransient<IPrivilegesService, PrivilegesService>();
        builder.Services.AddTransient<ITenantsService, TenantsService>();
        builder.Services.AddTransient<IUserAssociationsService, UserAssociationsService>();
        builder.Services.AddTransient<ILeadTicketAllowedActionChecker, LeadTicketAllowedActionChecker>();
        builder.Services.AddTransient<ICallAllowedActionChecker, CallAllowedActionChecker>();
        builder.Services.AddSingleton<ICachingService, CachingService>();
        builder.Services.AddSingleton<IUsersDataService, UsersDataService>();
        builder.Services.AddTransient<ICallsService, CallsService>();
        builder.Services.AddTransient<IPrimeTcrsService, PrimeTcrsService>();
        builder.Services.AddTransient<IPrimeTcrAllowedActionChecker, PrimeTcrAllowedActionChecker>();
        builder.Services.AddTransient<IClientsService, ClientsService>();
        builder.Services.AddTransient<IDataLoggingService, DataLoggingService>();
        builder.Services.AddTransient<ILeadTicketsActionsService, LeadTicketsActionsService>();
        builder.Services.AddTransient<ILeadTicketsService, LeadTicketsService>();
        builder.Services.AddTransient<ILookUpsService, LookUpsService>();
        builder.Services.AddTransient<INotificationsService, NotificationsService>();
        builder.Services.AddTransient<IDashboardService, DashboardService>();
        builder.Services.AddTransient<ISystemEventsService, SystemEventsService>();
        builder.Services.AddTransient<IPrimeTcrsActionsService, PrimeTcrsActionsService>();
        builder.Services.AddTransient<IUsersAuthService, UsersAuthService>();
        builder.Services.AddTransient<IUsersService, UsersService>();
        builder.Services.AddTransient<IUsersSessionService, UsersSessionService>();
    }

    private void InjectRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ICallsRepository, CallsRepository>();
        builder.Services.AddTransient<IClientsRepository, ClientsRepository>();
        builder.Services.AddTransient<ILeadTicketsRepository, LeadTicketsRepository>();
        builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
        builder.Services.AddTransient<ILookupsRepository, LookupsRepository>();
        builder.Services.AddTransient<INotificationsRepository, NotificationsRepository>();
        builder.Services.AddTransient<ISystemEventsRepository, SystemEventsRepository>();
        builder.Services.AddTransient<IUsersAuthRepository, UsersAuthRepository>();
        builder.Services.AddTransient<IUsersRepository, UsersRepository>();
        builder.Services.AddTransient<IUsersSessionsRepository, UsersSessionsRepository>();

        builder.Services.AddTransient<IDevelopersRepository, DevelopersRepository>();
        builder.Services.AddTransient<IPrivilegesRepository, PrivilegesRepository>();
        builder.Services.AddTransient<IOutsideBrokersRepository, OutsideBrokersRepository>();
        builder.Services.AddTransient<ITenantsRepository, TenantsRepository>();
        builder.Services.AddTransient<IPrimeTcrsRepository, PrimeTcrsRepository>();
        builder.Services.AddTransient<IUserAssociationsRepository, UserAssociationsRepository>();
    }

    private void InjectDbContexts(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration[LiteralsHelper.DefaultConnectionString];

        builder.Services.AddTransient<IAppDataContext>(_ => new AppDataContext(
            new DbContextOptionsBuilder<AppDataContext>().UseNpgsql(connectionString)

                .LogTo(Log.Logger.Information)
                .EnableDetailedErrors()
                  .EnableSensitiveDataLogging()
                .Options));

        builder.Services.AddTransient<ILookUpsDataContext>(_ => new LookUpsDataContext(
            new DbContextOptionsBuilder<LookUpsDataContext>().UseNpgsql(connectionString)

                .LogTo(Log.Logger.Information)
                .EnableDetailedErrors()
                 .EnableSensitiveDataLogging()
                .Options));

        builder.Services.AddTransient<IUsersSessionsDataContext>(_ => new UsersSessionsDataContext(
            new DbContextOptionsBuilder<UsersSessionsDataContext>().UseNpgsql(connectionString)

                                .LogTo(Log.Logger.Information)
                .EnableDetailedErrors()
                                .EnableSensitiveDataLogging()
                                .Options));

        builder.Services.AddTransient<IUsersDataContext>(_ => new UsersDataContext(
            new DbContextOptionsBuilder<UsersDataContext>().UseNpgsql(connectionString)

                .LogTo(Log.Logger.Information)
                .EnableDetailedErrors()
                 .EnableSensitiveDataLogging()
                .Options));

        builder.Services.AddTransient<ICrmDataContext>(_ => new CrmDataContext(
            new DbContextOptionsBuilder<CrmDataContext>().UseNpgsql(connectionString)
                .LogTo(Log.Logger.Information)
                .EnableDetailedErrors()
                 .EnableSensitiveDataLogging()
                .Options));
    }
}