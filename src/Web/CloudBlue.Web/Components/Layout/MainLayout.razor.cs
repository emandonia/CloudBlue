using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using CloudBlue.Web.Helpers;
using CloudBlue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace CloudBlue.Web.Components.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private async Task LogOut()
    {
        ShowhideSpinner(true);

        try
        {
            var apiKey = await LocalStorageHelper.RetrieveItem(LiteralsHelper.ApiKey);

            if (string.IsNullOrEmpty(apiKey) == false)
            {
                await UsersAuthService.RemoveSessionAsync(apiKey);
                await LocalStorageHelper.RemoveItem(LiteralsHelper.ApiKey);
            }

            CurrentUser = new LoggedInUserInfo();
            ShowhideSpinner(false);
            NavigationManager.NavigateTo("/login", true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #region Dependencies

    [Inject]
    public LoggedInUserInfo CurrentUser { set; get; } = null!;

    [Inject]
    protected IJSRuntime JsRuntime { get; set; } = null!;

    [Inject]
    protected IToastService ToastService { set; get; } = null!;

    [Inject]
    protected SpinnerService SpinnerService { get; set; } = null!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IUsersAuthService UsersAuthService { set; get; } = null!;

    [Inject]
    protected DialogService DialogService { get; set; } = null!;

    [Inject]
    protected TooltipService TooltipService { get; set; } = null!;

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; } = null!;

    [Inject]
    protected NotificationService NotificationService { get; set; } = null!;

    [Inject]
    private ILocalStorageHelper LocalStorageHelper { set; get; } = null!;

    #endregion Dependencies

    #region Private Methods and Events Handlers

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("startClock");
        }
        catch (Exception e)
        {

        }
    }

    private async Task<bool> CheckCurrentUser()
    {
        if (await IsAuthenticated() == false)
        {
            return false;
        }

        PopulateUserInfo();
        SetNavMenuFlags();

        if (_visibleContent == false)
        {
            _visibleContent = true;

            //                StateHasChanged();
        }

        return true;
    }

    protected override async Task OnParametersSetAsync()

    {
        try
        {
            await JsRuntime.InvokeVoidAsync("startClock");
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);
        }

        if (_isBusy == false && await CheckCurrentUser() == false && !NavigationManager.Uri.Contains("login"))
        {
            try
            {
                NavigateTo("/login");
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }

            await base.OnParametersSetAsync();

            return;
        }
        await ValidateUserAccess();
        await base.OnParametersSetAsync();

    }

    private readonly List<string> _allowedList = new() { "/", "/login", "/unauthorized", "/profile" };

    private async Task ValidateUserAccess()
    {
        var basePath = ExtractPath();

        if (basePath == "/logout")
        {
            await LogOut();

            return;
        }

        if (_allowedList.Contains(basePath))
        {
            return;
        }

        if (CurrentUser.UserId == 0)
        {
            NavigateTo("/login");
        }

        if (CurrentUser.Privileges.Any(z => z.Privilege == SystemPrivileges.ManageLookups && z.PrivilegeScope != PrivilegeScopes.Denied) && basePath.Length > 1 && basePath.IndexOf("lookups", StringComparison.Ordinal) > -1)
        {
            return;
        }


        if (CurrentUser.Privileges.Any(z => z.Path == basePath && z.PrivilegeScope != PrivilegeScopes.Denied) == false)
        {
            NavigateTo("/unauthorized");
        }
    }

    private string ExtractPath()
    {
        var path = new Uri(NavigationManager.Uri).AbsolutePath.ToLower();

        if (path.Length <= 1)
        {
            return path;
        }

        var idx = path.LastIndexOf('/');

        if (idx == -1 || idx >= path.Length)
        {
            return path;
        }

        if (long.TryParse(path.Substring(idx + 1), out _))
        {
            path = path.Substring(0, idx);
        }

        return path;
    }

    private async Task<bool> IsAuthenticated()
    {
        if (_isBusy)
        {
            return false;
        }

        _isBusy = true;
        string apiKey;

        try
        {
            apiKey = await LocalStorageHelper.RetrieveItem(LiteralsHelper.ApiKey);
        }
        catch (Exception e)
        {
            //Console.WriteLine(e);

            return false;
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }

        if (CurrentUser.UserId > 0 && CurrentUser.Privileges.Length > 0 && DateTime.UtcNow
               .Subtract(CurrentUser.LastUpdated)
               .TotalMinutes < 10)
        {
            return true;
        }

        var result = await UsersAuthService.PopulateUserSessionAsync(apiKey);

        return result;
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null)
    {
        TooltipService.Open(elementReference, _userFullTitleToolTip, options);
    }

    private readonly Dictionary<string, object> _onImageError = new();

    private void PopulateUserInfo()
    {
        if (string.IsNullOrEmpty(_userName))
        {
            _userName = CurrentUser.FullName;
            _userPosition = CurrentUser.UserPositionName ?? string.Empty;
        }

        if (string.IsNullOrEmpty(_userFullTitleToolTip))
        {
            _userFullTitleToolTip =
                $"{CurrentUser.UserPositionName}<br/> {CurrentUser.CompanyName}, {CurrentUser.BranchName}";
        }

        CurrentUser.UserImagePath =
            $"/user-images/{CurrentUser.CompanyId}/{CurrentUser.BranchId}/{CurrentUser.UserId}/{CurrentUser.UserId}___Selected.jpg";

        _onImageError.TryAdd("onerror", "this.onerror=null; this.src='/images/default_user_image.jpg';");
        ShowhideSpinner(false);
    }

    #endregion Private Methods and Events Handlers

    #region Private Properties

    private string _userName = string.Empty;
    private string _userPosition = string.Empty;
    private string _userFullTitleToolTip = string.Empty;
    private bool _isBusy;

    private bool _visibleContent;

    #region Timer

    private Timer? _timer;
    private double _hr, _min, _sec;

    // NOTE: this math can be simplified!!!
    private async Task SetClock()
    {
        var time = DateTime.Now;
        _hr = 360.0 * time.Hour / 12 + 30.0 * time.Minute / 60.0;
        _min = 360.0 * time.Minute / 60 + 6.0 * time.Second / 60.0;
        _sec = 360.0 * time.Second / 60 + 6.0 * time.Millisecond / 1000.0;
        _currentDate = DateTime.Now.ToString("dddd dd MMM, yyyy");
        await InvokeAsync(StateHasChanged);
    }

    private string _currentDate = DateTime.Now.ToString("dddd dd MMM, yyyy");

    #endregion Timer

    #endregion Private Properties

    #region Nav Menu Flags

    private bool _callsMenuFlag;
    private bool _callsAddMenuFlag;
    private bool _callsManageMenuFlag;
    private bool _adminMenuFlag;
    private bool _manageUsersMenuFlag;
    private bool _managePrivilegesMenuFlag;
    private bool _manageLookupsMenuFlag;

    private bool _leadTicketsMenuFlag;
    private bool _leadTicketsAddMenuFlag;
    private bool _leadTicketsManageMenuFlag;

    private bool _resaleRequestsMenuFlag;
    private bool _buyerRequestsMenuFlag;
    private bool _buyerRequestsAddMenuFlag;
    private bool _buyerRequestsManageMenuFlag;
    private bool _sellerRequestsMenuFlag;
    private bool _sellerRequestsAddMenuFlag;
    private bool _sellerRequestsManageMenuFlag;

    private bool _tcrsMenuFlag;
    private bool _primeTcrsMenuFlag;
    private bool _primeTcrsAddMenuFlag;
    private bool _primeTcrsManageMenuFlag;
    private bool _resaleTcrsMenuFlag;
    private bool _resaleTcrsAddMenuFlag;
    private bool _resaleTcrsManageMenuFlag;

    private void SetNavMenuFlags()
    {
        var privileges = CurrentUser.Privileges;

        if (privileges.Length == 0)
        {
            return;
        }

        _callsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.Calls);
        _adminMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.SystemAdministration);
        _managePrivilegesMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ManagePrivileges);
        _manageUsersMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ManageUsers);
        _manageLookupsMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ManageLookups);
        _callsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.CallsAdd);
        _callsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.CallsManage);
        _leadTicketsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.LeadTickets);
        _leadTicketsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.LeadTicketsAdd);
        _leadTicketsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.LeadTicketsManage);

        _resaleRequestsMenuFlag = privileges.Any(z =>
            z.PrivilegeCategory == PrivilegeCategories.ResaleBuyerRequests ||
            z.PrivilegeCategory == PrivilegeCategories.ResaleSellerRequests);

        _buyerRequestsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.ResaleBuyerRequests);
        _buyerRequestsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleBuyerRequestsAdd);
        _buyerRequestsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleBuyerRequestsManage);
        _sellerRequestsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.ResaleSellerRequests);
        _sellerRequestsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleSellerRequestsAdd);
        _sellerRequestsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleSellerRequestsManage);

        _tcrsMenuFlag = privileges.Any(z =>
            z.PrivilegeCategory == PrivilegeCategories.PrimeTcrs ||
            z.PrivilegeCategory == PrivilegeCategories.ResaleTcrs);

        _primeTcrsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.PrimeTcrs);
        _primeTcrsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.PrimeTcrsAdd);
        _primeTcrsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.PrimeTcrsManage);
        _resaleTcrsMenuFlag = privileges.Any(z => z.PrivilegeCategory == PrivilegeCategories.ResaleTcrs);
        _resaleTcrsAddMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleTcrsAdd);
        _resaleTcrsManageMenuFlag = privileges.Any(z => z.Privilege == SystemPrivileges.ResaleTcrsManage);
    }

    #endregion Nav Menu Flags

    #region Public

    public void ShowMessage(ToastLevel level, List<Errors> errors)
    {
        if (errors.Count == 0)
        {
            return;
        }

        var message = UtilityFunctions.ConstructMessage(errors);
        ShowMessage(level, message);
    }

    public void NavigateTo(string? url, bool newTab = false)
    {
        try
        {
            if (newTab)
            {
                JsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            else
            {
                NavigationManager.NavigateTo(url, false);
            }
        }
        catch (Exception e)
        {
        }
    }

    public async Task SetCurrentPageTitle(string title)
    {
        await JsRuntime.InvokeVoidAsync("setTitle", $"CloudBlue - {title}");
    }

    public void ShowhideSpinner(bool status)
    {
        if (status)
        {
            Console.WriteLine($"Spinner state: {status}");
            SpinnerService.Show();
        }
        else
        {
            Console.WriteLine($"Spinner state: {status}");
            SpinnerService.Hide();
        }
    }

    public void ShowMessage(ToastLevel level, string message)
    {
        ToastService.ShowToast(level, message);
    }

    public void ShowMessage(ToastLevel level, IEnumerable<string> messages)
    {
        ShowMessage(level, string.Join("; ", messages));
    }

    #endregion Public
}