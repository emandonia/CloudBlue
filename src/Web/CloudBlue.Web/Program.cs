using Blazored.LocalStorage;
using Blazored.Toast;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Components;
using CloudBlue.Web.Helpers;
using CloudBlue.Web.Services;
using Microsoft.Extensions.FileProviders;
using Radzen;
using Serilog;
using Serilog.Events;
using Z.BulkOperations;

var builder = WebApplication.CreateBuilder(args);
new ServicesBuilder().BuildServices(builder);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();
builder.Services.AddRazorPages();

var usersImagesPath = builder.Configuration[LiteralsHelper.UsersImagesPath];
var primeTcrsAttachmentsPath = builder.Configuration[LiteralsHelper.PrimeTcrsAttachmentsPath];
var tempFolderPath = builder.Configuration[LiteralsHelper.TempFolderPath];

var cloudBlueSettings =
    new CloudBlueSettings() { AttachmentsPath = primeTcrsAttachmentsPath, UsersImagesPath = usersImagesPath, TempFolderPath = tempFolderPath };
builder.Services.AddSingleton(cloudBlueSettings);


//var baseUrl = builder.Configuration[LiteralsHelper.ApiBaseUrl];
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

Log.Logger = new LoggerConfiguration().WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Error)
    .CreateLogger();

#region Dependencies

builder.Host.UseSerilog(Log.Logger);
builder.Services.AddScoped<LoggedInUserInfo>();
builder.Services.AddScoped<ILocalStorageHelper, LocalStorageHelper>();
builder.Services.AddScoped<SpinnerService>();

#endregion Dependencies
//bulk operations
Z.BulkOperations.LicenseManager.AddLicense("460;305-DELTAFOX", "1A7A178-29CD30D-A7CDDCC-09CA714-B9FF");
//ef extenstions
Z.BulkOperations.LicenseManager.AddLicense("646;105-DELTAFOX", "0229102-968913B-5796D00-7765745-063A");
var x = Z.BulkOperations.LicenseManager.ValidateLicense(ProviderType.PostgreSql);
x = Z.BulkOperations.LicenseManager.IsLicenseAdded();
x = Z.BulkOperations.LicenseManager.IsTrialExpired();
x = Z.BulkOperations.LicenseManager.IsTrialMode();

builder.Services.AddHealthChecks();


var app = builder.Build();
app.UseExceptionHandler("/Error", true);
app.UseHsts();
//app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(usersImagesPath!),
    RequestPath = "/user-images"
});
app.MapHealthChecks("/healthcheck");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();