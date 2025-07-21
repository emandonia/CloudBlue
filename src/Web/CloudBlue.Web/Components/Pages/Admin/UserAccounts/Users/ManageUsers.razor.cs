using Blazored.Toast.Services;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Web.Components.Layout;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CloudBlue.Web.Components.Pages.Admin.UserAccounts.Users;

public partial class ManageUsers : ComponentBase
{
    private const int SalesDeptId = 1;
    private IEnumerable<AgentItem> _allTopManagers = [];
    private IEnumerable<LookupItem<int>> _allUserPositions = [];
    private LoadDataArgs _args;
    private IEnumerable<LookupItem<int>> _branches = [];
    private IEnumerable<LookupItem<int>> _companies = [];
    private IEnumerable<LookupItem<int>> _departments = [];
    private UsersFiltersModel _filters = new();
    [Inject]
    private NotificationService NotificationService { set; get; } = null!;

    private RadzenDataGrid<UserItemForList> _grid0;

    private bool _isBusy;
    private IEnumerable<LookupItem<int>> _topManagers = [];

    private bool _topManagersDisabled;
    private int _totalNumber;
    private IEnumerable<LookupItem<int>> _userPositions = [];

    private IEnumerable<UserItemForList> _users = [];
    private LookupItem<int>[] _yesNoList = [];

    [Inject]
    private ILookUpsService LookUpsService { get; set; } = null!;

    [Inject]
    private DialogService DialogService { set; get; } = null!;

    [CascadingParameter]
    public MainLayout Layout { get; set; } = null!;

    [Inject]
    public IUsersService UsersService { get; set; } = null!;

    private bool IsBusy
    {
        set
        {
            _isBusy = value;
            Layout.ShowhideSpinner(_isBusy);
        }
        get => _isBusy;
    }

    private async Task Search(MouseEventArgs obj)
    {
        if (_args == null)
        {
            _args = new LoadDataArgs { Skip = 0 };
        }

        await LoadData(_args);
    }

    private async Task ResetFilters()
    {
        if (IsBusy)
            return;

        IsBusy = true;
        //  await UsersService.UpdateManagersArraysAsync();

        _filters = new UsersFiltersModel();
        _args.Skip = 0;
        _args.Filter = null;
        await LoadData(_args);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadData(new LoadDataArgs());
        await PopulateLookups();
    }

    private async Task PopulateLookups()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        // await UsersService.UpdateManagersArraysAsync();
        _departments = await LookUpsService.GetDepartmentsAsync();
        _allUserPositions = await LookUpsService.GetUserPositionsAsync();
        _companies = await LookUpsService.GetCompaniesAsync();
        _yesNoList = await LookUpsService.GetYesNoList();

        _allTopManagers =
            await LookUpsService.GetSalesPersonsAsync([SalesPersonClasses.TopManager, SalesPersonClasses.Manager], false);

        _userPositions = _allUserPositions;
        IsBusy = false;
    }

    protected async Task AddButtonClick(MouseEventArgs args)
    {
        await DialogService.OpenAsync<CreateUser>("Create User");
        await LoadData(_args);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Layout.SetCurrentPageTitle("Manage Users");
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _args = args;

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        _filters.ExtraFilters = args.Filter;

        if (args.Top is > 0)
        {
            _filters.PageSize = args.Top.Value;
        }

        _filters.PageIndex = (args.Skip ?? 0) / _filters.PageSize;

        if (string.IsNullOrEmpty(args.OrderBy) == false && args.OrderBy.Length > 0 &&
           args.OrderBy.IndexOf(" ", StringComparison.Ordinal) > 0 && args.OrderBy.Split(" ")
               .Length == 2)
        {
            var arr = args.OrderBy.Split(" ");
            _filters.SortField = arr[0];
            _filters.SortDirection = arr[1];
        }

        var retObj = await UsersService.GetUsersAsync(_filters);
        _totalNumber = retObj.TotalCount;
        _users = retObj.Items;
        IsBusy = false;
    }

    private void DepartmentChanged()
    {
        _filters.PositionId = 0;
        _userPositions = [];

        _userPositions = _allUserPositions
            .Where(z => _filters.DepartmentId == 0 || z.ParentItemId == _filters.DepartmentId)
            .ToArray();

        PopulateTopManagers();
    }

    private void PopulateTopManagers()
    {
        _topManagers = [];
        _filters.TopManagerId = 0;

        if (_filters.DepartmentId != SalesDeptId)
        {
            _topManagersDisabled = true;

            return;
        }

        _topManagersDisabled = false;

        _topManagers = _allTopManagers.Where(z =>
                z.SalesPersonClass == SalesPersonClasses.TopManager &&
                (_filters.CompanyId == 0 || z.CompanyId == _filters.CompanyId) &&
                (_filters.BranchId == 0 || z.BranchId == _filters.BranchId))
            .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, "", 0))
            .ToArray();
    }

    private void CompanyChanged()
    {
        _branches = [];
        _filters.BranchId = 0;
        var companyItem = _companies.FirstOrDefault(z => z.ItemId == _filters.CompanyId);

        if (companyItem == null)
        {
            return;
        }

        _branches = companyItem.SubLookUps;
        PopulateTopManagers();
    }

    private async Task ApplySingleAction(RadzenSplitButtonItem? item)
    {
        if (item == null || item.Value == null)
        {
            return;
        }

        var itemValue = item.Value;

        if (string.IsNullOrEmpty(itemValue))
        {
            return;
        }

        var idx = itemValue.IndexOf("_", StringComparison.Ordinal);

        if (idx == -1)
        {
            return;
        }

        var actionId = 0;
        var arr = itemValue.Split('_');
        var result = int.TryParse(arr[1], out actionId);

        if (result == false || actionId == 0 ||
           Enum.TryParse(actionId.ToString(), out ManageUserActions action) == false)
        {
            return;
        }

        IsBusy = true;
        var userId = 0;
        result = int.TryParse(arr[0], out userId);
        var actionResult = false;
        var showSuccess = true;

        switch (action)
        {
            case ManageUserActions.ActivateAccount:
                actionResult = await UsersService.UpdateAccountStatusAsync(userId, true);

                break;

            case ManageUserActions.DeactivateAccount:
                actionResult = await UsersService.UpdateAccountStatusAsync(userId, false);

                break;

            case ManageUserActions.UnblockAccount:
                actionResult = await UsersService.UnblockAccountAsync(userId);

                break;

            case ManageUserActions.ResetPassword:
                var res = await DialogService.Confirm("Are you sure you want Reset Password?", "Reset User Password",
      new ConfirmOptions { OkButtonText = "Yes", CancelButtonText = "No" });

                if (res != null && res.Value)
                {
                    actionResult = await UsersService.ResetUserPasswordAsync(userId);
                }

                break;

            default:
                IsBusy = false;
                showSuccess = false;
                await ShowUpdateDialogBox(userId, action);

                break;
        }

        IsBusy = false;

        if (showSuccess == false)
        {
            return;
        }

        if (actionResult)
        {
            Layout.ShowMessage(ToastLevel.Success, "Action has applied successfully");
        }
        else
        {
            Layout.ShowMessage(ToastLevel.Error, UsersService.LastErrors);
        }
    }

    private async Task ShowUpdateDialogBox(int userId, ManageUserActions action)
    {
        LookupItem<int>[] lookupItems = [];
        var currentUser = _users.FirstOrDefault(z => z.Id == userId);

        if (currentUser == null)
        {
            return;
        }

        if (action == ManageUserActions.UpdatePosition)
        {
            lookupItems = _allUserPositions.Where(z => z.ParentItemId == currentUser.DepartmentId)
                .ToArray();
        }
        else if (action == ManageUserActions.UpdateDirectManager)
        {
            if (currentUser.DepartmentId != SalesDeptId)
            {
                return;
            }

            lookupItems = _allTopManagers.Where(z =>
                    (currentUser.CompanyId == 0 || z.CompanyId == currentUser.CompanyId) &&
                    (currentUser.BranchId == 0 || z.BranchId == currentUser.BranchId))
                .Select(z => new LookupItem<int>(z.AgentName, z.AgentId, "", 0))
                .ToArray();
        }

        var width = "600px";
        var height = "300px";

        await DialogService.OpenAsync<UpdateUserPopUp>("Update User",
            new Dictionary<string, object>
            {
                { "CurrentUser", currentUser },
                { "Action", action },
                { "Title", "Update User" },
                { "LookupItems", lookupItems }
            }, new DialogOptions { Width = width, Height = height });

        await LoadData(_args);
    }
}